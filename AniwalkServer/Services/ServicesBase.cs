using AniwalkServer.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace AniwalkServer.Services
{
    /// <summary>
    /// 服務的底層
    /// </summary>
    public abstract class ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        protected ServicesBase(AniwalkDBContext Context)
        {
            Debug.WriteLine($"[{GetType().Name}] Constructor called.");
            this.Context = Context;
        }
    }
}
