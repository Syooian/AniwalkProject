using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace AniwalkServer.Services
{
    public class CommentsServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public CommentsServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="IncludeDeleted"></param>
        /// <returns></returns>
        public async Task<List<Comments>> GetComments(int VisitSN, bool IncludeDeleted = false)
        {
            var SQL = Context.Comments.Where(V => V.SN == VisitSN);

            if (!IncludeDeleted)
                SQL = SQL.Where(D => D.DeleteDate == null);

            var Result = await SQL
                .Include(V => V.Replies).ThenInclude(R => R.Member)
                .Include(V => V.Replies).ThenInclude(R => R.ParentReply)
                .OrderByDescending(C => C.CommentDate)
                .ToListAsync();

            //把回覆依時間排序
            foreach (var Comment in Result)
            {
                //Debug.WriteLine($"Comment {Comment.CommentID} R : {Comment.Replies == null}");
                Comment.Replies = Comment.Replies.OrderByDescending(R => R.ReplyDate).ToList();
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <param name="IncludeDeleted"></param>
        /// <returns></returns>
        public async Task<PageDTO<Comments>> GetComments(int VisitSN, int Page = 1, int PageSize = 0, bool IncludeDeleted = false)
        {
            //查詢條件參數
            var SQLPara = new DynamicParameters();
            SQLPara.Add("@VisitSN", VisitSN);

            /*
             select count(*) from Comments as C
	            left join Members as M on C.MemberID = M.MemberID
	            left join Replies as R on R.CommentID = C.CommentID
	            left join Members as RM on R.MemberID = RM.MemberID
	            left join Replies as PR on R.ParentReplyID = PR.ReplyID
	            left join Members as PRM on PR.MemberID = PRM.MemberID
	            where C.SN = 1
            select 
	            C.*, 
	            M.Name AS MemberName,
	            R.*, 
	            RM.Name AS ReplyMemberName,
	            PR.*,
	            PRM.Name AS ParentReplyMemberName
	            from Comments as C
	            left join Members as M on C.MemberID = M.MemberID
	            left join Replies as R on R.CommentID = C.CommentID
	            left join Members as RM on R.MemberID = RM.MemberID
	            left join Replies as PR on R.ParentReplyID = PR.ReplyID
	            left join Members as PRM on PR.MemberID = PRM.MemberID
	            where C.SN = 1
	            order by C.CommentDate desc, R.ReplyDate desc
             */

            //資料Join
            var SQLJoin = @$"
                left join Members as M on C.MemberID = M.MemberID
                left join Replies as R on R.CommentID = C.CommentID
	            left join Members as RM on R.MemberID = RM.MemberID
	            left join Replies as PR on R.ParentReplyID = PR.ReplyID
	            left join Members as PRM on PR.MemberID = PRM.MemberID
                ";

            var SQLSelect = "where C.SN = @VisitSN ";
            if (!IncludeDeleted)
                SQLSelect += "and C.DeleteDate = null and R.DeleteDate = null ";

            //總數查詢
            var SQLCount = "select count(*) ";

            //資料查詢
            var SQLData = @$"select 
                C.*,
                M.Name AS MemberName,
	            R.*, 
	            RM.Name AS ReplyMemberName,
	            PR.*,
	            PRM.Name AS ParentReplyMemberName
                from Comments as C
                ";

            SQLCount += SQLJoin + SQLSelect;
            SQLData += SQLJoin + SQLSelect + "order by C.CommentDate desc, R.ReplyDate desc";

            #region 加入數量查詢和分頁查詢參數
            if (PageSize == 0)//不篩選
            {
                SQLData += ";";

                SQLCount += "from Comments as C;";
            }
            else
            {
                if (Page < 1)//防呆
                    Page = 1;


            }
            #endregion

            var Connection = Context.Database.GetDbConnection();

            try
            {
                var Result = await Connection.QueryMultipleAsync(SQLCount + SQLData, SQLPara, commandType: CommandType.Text);

                //接收資料
                var Data = new PageDTO<Comments>(
                    Result,
                    Page,//當前頁碼
                    PageSize//每頁筆數
                );

                return Data;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public async Task<Comments> GetComment(string CommentID)
        {
            return await Context.Comments.FindAsync(CommentID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public async Task<Result> DeleteComment(string CommentID)
        {
            var Comment = await GetComment(CommentID);
            if (Comment == null)
                return new Result(ResultType.Fail, "Not Found");

            Comment.DeleteDate = DateTime.Now;

            Context.Update(Comment);
            await Context.SaveChangesAsync();

            return new Result();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCommentsExists(string id)
        {
            return Context.Comments.Any(e => e.CommentID == id);
        }
    }
}
