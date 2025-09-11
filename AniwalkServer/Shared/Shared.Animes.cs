using System.Diagnostics;

namespace AniwalkServer
{
    public partial class Shared
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <returns></returns>
        public static string GetAnimeHeaderPhotoPath(string AnimeID)
        {
            var PhotosDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", AnimesPhotosRootPath, AnimeID);
            string HeaderPhotoPath = null;
            if (Directory.Exists(PhotosDir))
            {
                var PhotosPath = Directory.GetFiles(PhotosDir);

                var HeaderIndex = Array.FindIndex(PhotosPath, N => N.Contains("Header"));
                if (HeaderIndex != -1)
                {
                    HeaderPhotoPath = GetAnimesPhotosPath(AnimeID, Path.GetFileName(PhotosPath[HeaderIndex]));
                }
            }

            return HeaderPhotoPath;
        }
    }
}
