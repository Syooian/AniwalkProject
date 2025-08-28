using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class AddNewAnimesServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public AddNewAnimesServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AddNewAnime>> GetAddNewAnimes()
        {
            return await Context.AddNewAnimes
                .OrderBy(A => A.AddDate)
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<AddNewAnime> GetAddNewAnime(int SN)
        {
            return await Context.AddNewAnimes.FindAsync(SN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public bool IsAddNewAnimeExists(int SN)
        {
            return Context.AddNewAnimes.Any(e => e.SN == SN);
        }
    }
}
