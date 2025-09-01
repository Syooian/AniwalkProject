namespace AniwalkServer.QueryParameters
{
    public class MapDataParam
    {
        /// <summary>
        /// 地圖寬
        /// <para>需含單位 (e.g. px, vw, vh...etc)</para>
        /// </summary>
        public string MapWidth { get; set; }
        /// <summary>
        /// 地圖高
        /// <para>需含單位 (e.g. px, vw, vh...etc)</para>
        /// </summary>
        public string MapHeight { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public int Zoom { get; set; } = 10;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MapCenterLatitude + "," + MapCenterLongitude + ", Zoom : " + Zoom;
        }
    }
}
