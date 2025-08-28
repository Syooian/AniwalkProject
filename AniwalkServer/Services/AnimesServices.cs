using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class AnimesServices : ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public AnimesServices(AniwalkDBContext Context) : base(Context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <returns></returns>
        public async Task<SelectList> GetAnimesSelect(string? AnimeID = null)
        {
            var Result = await Context.Animes.OrderBy(A => A.Title).ToListAsync();

            return new SelectList(Result, nameof(Animes.AnimeID), nameof(Animes.Title), AnimeID);
        }
    }
}
