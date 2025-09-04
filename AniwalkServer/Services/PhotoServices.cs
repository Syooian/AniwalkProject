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

        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <param name="Visit"></param>
        /// <param name="SetDBFirst">上傳圖片時一併新增資料進資料庫</param>
        /// <param name="VisitPhotos"></param>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<Result> UploadPhoto(Visits Visit, bool SetDBFirst, List<VisitsPhotosDTO>? VisitPhotos, string MemberID)
        {
            if (VisitPhotos == null || VisitPhotos.Count == 0)
            {
                return new Result(Message: "沒有上傳圖片");
            }

            var UploadPhotos = VisitPhotos.FindAll(VP => VP.UploadFile != null && VP.UploadFile.Length != 0);
            foreach (var Photo in UploadPhotos)
            {
                //檢查檔案類型
                switch (Photo.UploadFile.ContentType)
                {
                    case "image/gif":
                    case "image/bmp":
                    case "image/jpg":
                    case "image/jpeg":
                    case "image/png":
                    case "image/jfif":
                        break;
                    default:
                        return new Result(ResultType.Fail, "有不支援的圖片類型");
                }

                try
                {
                    //上傳路徑
                    var UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, MemberID);
                    //Debug.WriteLine($"UploadPath : {UploadPath}");
                    //檢查上傳路徑
                    if (!Directory.Exists(UploadPath))
                        Directory.CreateDirectory(UploadPath);
                    //上傳
                    using (FileStream FS = new FileStream(Path.Combine(UploadPath, Photo.PhotoID + Photo.PhotoType), FileMode.Create))
                    {
                        await Photo.UploadFile.CopyToAsync(FS);
                    }

                    if (SetDBFirst)
                    {
                        var OrderID = VisitPhotos.FindIndex(P => P.PhotoID == Photo.PhotoID);

                        Context.Add(new VisitsPhotos()
                        {
                            PhotoID = Photo.PhotoID,
                            PhotoType = Photo.PhotoType,
                            Description = Photo.Description,
                            MemberID = MemberID,
                            SN = Visit.SN,
                            SortNumber = OrderID
                        });

                        Debug.WriteLine($"新增圖片 {Photo.PhotoID}, OrderID : " + OrderID);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"UploadPhoto ex : {ex.Message}");
                    return new Result(ResultType.Fail, "上傳失敗");
                }
            }

            return new Result();
        }

        /// <summary>
        /// 更新圖片資料
        /// </summary>
        /// <param name="Visit"></param>
        /// <param name="VisitPhotos"></param>
        /// <returns></returns>
        public async Task<Result> UpdatePhotoData(Visits Visit, List<VisitsPhotosDTO>? VisitPhotos)
        {
            if (VisitPhotos == null || VisitPhotos.Count == 0)
            {
                return new Result(Message: "沒有圖片資料更新");
            }

            //Debug.WriteLine($"UpdatePhotoData VisitPhotos Count : {VisitPhotos.Count()}");
            var UpdatePhotoData = VisitPhotos.FindAll(VP => VP.UploadFile == null);
            foreach (var PhotoData in UpdatePhotoData)
            {
                //Debug.WriteLine(VP.ToString());
                var Original = await Context.VisitsPhotos.FirstOrDefaultAsync(V => V.PhotoID == PhotoData.PhotoID);
                var OrderID = VisitPhotos.FindIndex(P => P.PhotoID == PhotoData.PhotoID);
                if (Original != null && (Original.Description != PhotoData.Description || Original.SortNumber != OrderID))//檢查資料是否有變動
                {
                    //Debug.WriteLine($"修改圖片資料 {Original.PhotoID}");
                    Original.Description = PhotoData.Description;
                    Original.SortNumber = OrderID;

                    Context.Update(Original);
                }
            }

            return new Result();
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
            if (Visit.VisitsPhotos == null || Visit.VisitsPhotos.Count == 0)
                return; //如果沒有照片則不執行

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
