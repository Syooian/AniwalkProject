using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class RepliesServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public RepliesServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReplyID"></param>
        /// <returns></returns>
        public async Task<Replies> GetReply(string ReplyID)
        {
            return await Context.Replies.FindAsync(ReplyID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReplyID"></param>
        /// <returns></returns>
        public async Task<Result> DeleteReply(string ReplyID)
        {
            var Reply = await GetReply(ReplyID);
            if (Reply == null)
                return new Result(ResultType.Fail, "Not Found");

            Reply.DeleteDate = DateTime.Now;

            Context.Update(Reply);
            await Context.SaveChangesAsync();

            return new Result();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsReplyExists(string id)
        {
            return Context.Replies.Any(e => e.ReplyID == id);
        }
    }
}
