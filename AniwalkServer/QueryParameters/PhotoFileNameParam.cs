namespace AniwalkServer.QueryParameters
{
    public class PhotoFileNameParam
    {
        public string PhotoID { get; set; }
        public string PhotoType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PhotoID"></param>
        /// <param name="PhotoType"></param>
        public PhotoFileNameParam(string PhotoID, string PhotoType)
        {
            this.PhotoID = PhotoID;
            this.PhotoType = PhotoType;
        }

        public override string ToString()
        {
            return PhotoID + PhotoType;
        }
    }
}
