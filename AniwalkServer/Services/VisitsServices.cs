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
        /// <param name="SortVisitsPhotos">對到訪紀錄照片做排序</param>
        /// <returns></returns>
        public async Task<List<Visits>> GetVisits(bool SortVisitsPhotos = false)
        {
            var Visits = await Context.Visits
                    .Include(V => V.Member)
                    .Include(V => V.Anime)
                    .Include(V => V.Country)
                    .Include(V => V.VisitsPhotos)
                    .OrderByDescending(V => V.CreatedDate)
                    .ToListAsync();

            if (SortVisitsPhotos)
            {
                foreach (var Visit in Visits)
                {
                    Visit.VisitsPhotos = SortVisitPhotos(Visit.VisitsPhotos);
                }
            }

            return Visits;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="SortVisitsPhotos">對到訪紀錄照片做排序</param>
        /// <returns></returns>
        public async Task<Visits> GetVisit(int VisitSN, bool SortVisitsPhotos = false)
        {
            var Visit = await Context.Visits
                .Include(V => V.Member)
                .Include(V => V.Anime)
                .Include(V => V.Country)
                .Include(V => V.VisitsPhotos)
                .OrderByDescending(V => V.CreatedDate)
                .FirstOrDefaultAsync(V => V.SN == VisitSN);

            if (SortVisitsPhotos)
            {
                Visit.VisitsPhotos = SortVisitPhotos(Visit.VisitsPhotos);
            }

            return Visit;
        }

        /// <summary>
        /// 排序到訪紀錄照片
        /// </summary>
        /// <param name="VisitPhotos"></param>
        List<VisitsPhotos>? SortVisitPhotos(List<VisitsPhotos>? VisitPhotos)
        {
            if (VisitPhotos == null)
                return null;

            return VisitPhotos.OrderBy(N => N.SortNumber).ToList();
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
