namespace AniwalkServer.DTOs
{
    public class VisitsDTO
    {
        public int SN { get; set; }
        public string MainText { get; set; } = null!;
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;
        public DateTime VisitedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// <see cref="AniwalkServer.Models.Members.MemberID"/>
        /// </summary>
        public string MemberID { get; set; } = null!;
        /// <summary>
        /// 
        /// <see cref="AniwalkServer.Models.Members.Name"/>
        /// </summary>
        public string Name { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        /// <summary>
        /// 
        /// <see cref="AniwalkServer.Models.Animes.AnimeID"/>
        /// </summary>
        public string AnimeID { get; set; } = null!;
        /// <summary>
        /// 
        /// <see cref="AniwalkServer.Models.Animes.Title"/>
        /// </summary>
        public string Title { get; set; } = null!;
    }
}
