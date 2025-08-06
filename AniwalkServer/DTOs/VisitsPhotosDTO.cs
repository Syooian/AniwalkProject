namespace AniwalkServer.DTOs
{
    public class VisitsPhotosDTO
    {
        /// <summary>
        /// 照片ID
        /// </summary>
        public string PhotoID { get; set; } = null!;
        /// <summary>
        /// 照片類型
        /// <para>副檔名</para>
        /// </summary>
        public string PhotoType { get; set; } = null!;
        /// <summary>
        /// 說明
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 上傳圖片
        /// </summary>
        public IFormFile? UploadFile { get; set; }

        public override string ToString()
        {
            return $"PhotoID : {PhotoID}, PhotoType : {PhotoType}, Description : {Description}, UploadFile : {(UploadFile == null ? "Null" : UploadFile.Name)}";
        }
    }
}
