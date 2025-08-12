using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class AnnouncementsServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public AnnouncementsServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<List<Announcements>> GetAnnouncements(int? Skip, int? Take)
        {
            var Result = Context.Announcements.OrderByDescending(A => A.CreatedDate);

            if (Skip != null && Take != null)
                Result.Skip((int)Skip).Take((int)Take);

            return await Result.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<Announcements> GetAnnouncement(int SN)
        {
            return await Context.Announcements.FindAsync(SN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAnnouncementsExists(int id)
        {
            return Context.Announcements.Any(e => e.SN == id);
        }
    }
}
