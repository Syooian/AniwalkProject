using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class VisitsServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;

        public VisitsServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 到訪紀錄是否存在
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        private bool IsVisitExists(int VisitSN)
        {
            return Context.Visits.Any(V => V.SN == VisitSN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Visits>> GetVisits()
        {
            return await Context.Visits.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<Visits> GetVisit(int VisitSN)
        {
            var Visit = await Context.Visits
                .Include(V => V.Member)
                .Include(V => V.Anime)
                .Include(V => V.Country)
                .Include(V => V.VisitsPhotos)
                .FirstOrDefaultAsync(V => V.SN == VisitSN);

            return Visit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Visit"></param>
        /// <returns></returns>
        public async Task<Visits> UpdateVisit(Visits Visit)
        {
            Context.Entry(Visit).State = EntityState.Modified;

            try
            {
                //Context.Update(Visit);

                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Visit;
        }
    }
}
