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
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<Comments> GetComment(string VisitSN)
        {
            return await Context.Comments.FindAsync(VisitSN);
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
