namespace AniwalkServer.Services
{
    /// <summary>
    /// 管理圖片上傳, 刪除的服務
    /// </summary>
    public class PhotoServices
    {
        /// <summary>
        /// 刪除照片
        /// </summary>
        /// <param name="PhotoFileName">照片檔名<para>含副檔名</para></param>
        public void DeletePhoto(string MemberID, List<string> PhotoFileName)
        {
            for (int a = 0; a < PhotoFileName.Count; a++)
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, MemberID, PhotoFileName[a]);

                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath); //刪除照片檔案
                }
            }
        }
    }
}
