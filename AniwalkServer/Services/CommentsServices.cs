using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

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
