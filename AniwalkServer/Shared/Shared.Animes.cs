using System.Diagnostics;

namespace AniwalkServer
{
    public partial class Shared
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AnimeHeaderPhotoName = "Header";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <returns></returns>
        public static string GetAnimeHeaderPhotoPath(string AnimeID)
        {
            string HeaderPhotoPath = null;
            if (AnimeID != null)
            {
                var PhotosDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", AnimesPhotosRootPath, AnimeID);

                if (Directory.Exists(PhotosDir))
                {
                    var PhotosPath = Directory.GetFiles(PhotosDir);

                    var HeaderIndex = Array.FindIndex(PhotosPath, N => Path.GetFileNameWithoutExtension(N) == AnimeHeaderPhotoName);
                    if (HeaderIndex != -1)
                    {
                        HeaderPhotoPath = GetAnimesPhotosPath(AnimeID, Path.GetFileName(PhotosPath[HeaderIndex]));
                    }
                }
            }

            return HeaderPhotoPath;
        }
    }
}
