namespace AniwalkServer.QueryParameters
{
    public class MapDataParam
    {
        /// <summary>
        /// 地圖寬
        /// </summary>
        public int MapWidth { get; set; }
        /// <summary>
        /// 地圖高
        /// </summary>
        public int MapHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableClick { get; set; }
        /// <summary>
        /// 地圖中心點
        /// </summary>
        public double MapCenterLatitude { get; set; } = 22.593469753520136;
        /// <summary>
        /// 地圖中心點
        /// </summary>
        public double MapCenterLongitude { get; set; } = 120.3088710110155;
    }
}
