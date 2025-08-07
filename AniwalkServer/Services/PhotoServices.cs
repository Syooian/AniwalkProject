using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AniwalkServer.Services
{
    /// <summary>
    /// 管理圖片上傳, 刪除的服務
    /// </summary>
    public class PhotoServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public PhotoServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        #region 刪除
        /// <summary>
        /// 刪除既有照片
        /// </summary>
        /// <param name="Visit"></param>
        /// <param name="VisitPhotos"></param>
        public async Task<string> DeletePhoto(Visits Visit, List<VisitsPhotosDTO>? VisitPhotos)
        {
            if (VisitPhotos != null)
            {
                //找出原到訪紀錄照片與要刪除的照片檔名 (資料存在於原資料但不存在於回傳的DTO中)
                //var DeletePhotoList = Context.VisitsPhotos
                //    .Where(P => P.SN == Visit.SN && !VisitPhotos.Any(VP => VP.PhotoID == P.PhotoID))
                //    .Select(P => new PhotoFileNameParam(P.PhotoID, P.PhotoType))
                //    .ToList();

                //DeletePhoto(Visit, DeletePhotoList);
                //這個例外是因為 EF Core 無法將 VisitPhotos.Any(...) 這種「跨集合比對」的 LINQ 寫法轉換成 SQL。
                //解決方法是：先將資料庫資料取出到記憶體，再用 LINQ 比對。

                //先找出該SN的所有照片
                var Original = await Context.VisitsPhotos.Where(V => V.SN == Visit.SN).ToListAsync();
                if (Original != null)
                {
                    //比對出原有照片與要刪除的照片 (不存在於DTO內)
                    var DeletePhotoList = Original.Where(P => !VisitPhotos.Any(VP => VP.PhotoID == P.PhotoID)).ToList();

                    DeletePhoto(Visit, DeletePhotoList.Select(P => new PhotoFileNameParam(P.PhotoID, P.PhotoType)).ToList());
                }
            }
            else
            {
                if (Visit.VisitsPhotos == null)
                    Debug.WriteLine("Visit is null");
                if (VisitPhotos == null)
                    Debug.WriteLine("VisitPhotos is null");
            }

            return await Task.FromResult("");
        }
        /// <summary>
        /// 刪除到訪記錄的全部照片
        /// </summary>
        /// <param name="Visit"></param>
        public void DeletePhoto(Visits Visit)
        {
            DeletePhoto(Visit, Visit.VisitsPhotos.Select(P => new PhotoFileNameParam(P.PhotoID, P.PhotoType)).ToList());
        }
        /// <summary>
        /// 刪除指定照片
        /// </summary>
        /// <param name="PhotoFileName">照片檔名<para>含副檔名</para></param>
        public void DeletePhoto(Visits Visit, List<PhotoFileNameParam> PhotoFileName)
        {
            for (int a = 0; a < PhotoFileName.Count; a++)
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, Visit.MemberID, PhotoFileName[a].ToString());

                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath); //刪除照片檔案
                }

                //刪除資料庫資料 (寫法不佳)
                Context.VisitsPhotos.Remove(Context.VisitsPhotos.FirstOrDefault(V => V.PhotoID == PhotoFileName[a].PhotoID));
            }
        }
        #endregion
    }
}
