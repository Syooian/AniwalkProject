using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class CountriesServices : ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public CountriesServices(AniwalkDBContext Context) : base(Context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns></returns>
        public async Task<SelectList> GetCountriesSelect(string? CountryCode = null)
        {
            var Result = await Context.Countries.OrderBy(C => C.CountryName).ToListAsync();

            return new SelectList(Result, nameof(Countries.CountryCode), nameof(Countries.CountryName), CountryCode);
        }
    }
}
