using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;
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
