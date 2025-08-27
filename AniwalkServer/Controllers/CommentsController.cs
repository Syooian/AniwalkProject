using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AniwalkServer.Data;
using AniwalkServer.Services;
using System.Diagnostics;

namespace AniwalkServer.Controllers
{
    [Authorize(Roles = Shared.Role_Member)]
    public class CommentsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        readonly CommentsServices CommentsServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CommentsServices"></param>
        public CommentsController(AniwalkDBContext context, CommentsServices CommentsServices)
        {
            _context = context;
            this.CommentsServices = CommentsServices;
        }

        // GET: Comments/Create?VisitSN=V
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public IActionResult Create(int VisitSN)
        {
            Console.WriteLine($"Create VisitSN : {VisitSN}");

            //ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN");
            ViewData["SN"] = VisitSN;

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentID,CommentContent,MemberID,SN")] Comments comments)
        {
            Console.WriteLine($"Create CommentID : {comments.CommentID}, CommentText : {comments.CommentContent}, CommentDate : {comments.CommentDate}, MemberID : {comments.MemberID}, SN: {comments.SN}");

            if (ModelState.IsValid)
            {
                //指定當前時間為留言日期
                comments.CommentDate = DateTime.Now;

                _context.Add(comments);
                await _context.SaveChangesAsync();
                return Json(comments);
            }
            //ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", comments.MemberID);
            //ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
            //ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN", comments.SN);

            //Shared.ShowModelState(ModelState);

            return Json(comments);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await CommentsServices.GetComment(id);

            if (comments == null)
            {
                return NotFound();
            }

            //ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", comments.MemberID);
            //ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
            //ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN", comments.SN);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CommentID,CommentContent,CommentDate,MemberID,SN")] Comments comments)
        {
            if (id != comments.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentsServices.IsCommentsExists(comments.CommentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", comments.MemberID);
            //ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
            ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN", comments.SN);
            return View(comments);
        }

        // POST: Comments/Delete/5
        /// <summary>
        /// 刪除評論
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int VisitSN, string CommentID)
        {
            //Debug.WriteLine("DeleteComment : " + CommentID);

            if (string.IsNullOrEmpty(CommentID))
            {
                return NotFound();
            }

            var Result = await CommentsServices.DeleteComment(CommentID);
            if (Result.Type == ResultType.Fail)
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return ViewComponent("VC_Comment", new { VisitSN = VisitSN });
        }
    }
}
