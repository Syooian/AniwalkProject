using AniwalkServer.Data;

namespace AniwalkServer.Services
{
    public class ForgotPasswordServices
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ForgotPasswordServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }
    }
}
