using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AniwalkServer.Data;
using AniwalkServer.Services;

namespace AniwalkServer.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area(Shared.Role_Admin), Authorize(Roles = Shared.Role_Admin)]
    public class AddNewAnimesController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        AddNewAnimesServices AddNewAnimesServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="AddNewAnimesServices"></param>
        public AddNewAnimesController(AniwalkDBContext Context, AddNewAnimesServices AddNewAnimesServices)
        {
            this.Context = Context;
            this.AddNewAnimesServices = AddNewAnimesServices;
        }

        // GET: AddNewAnimes
        /// <summary>
        /// 檢視新增動畫建議
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var Result = await AddNewAnimesServices.GetAddNewAnimes();

            return View(Result);
        }
    }
}
