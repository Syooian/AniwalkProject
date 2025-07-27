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

namespace AniwalkServer.Controllers
{
    [Authorize(Roles = Shared.Role_Member)]
    public class CommentsController : Controller
    {
        private readonly AniwalkDBContext _context;

        public CommentsController(AniwalkDBContext context)
        {
            _context = context;
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

            Console.WriteLine("ModelState is not valid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                //印出模型驗證錯誤的訊息
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }

            return Json(comments);
        }

        /// <summary>
        /// 取得回覆留言資料
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public IActionResult GetContentsByViewComponent(int VisitSN)
        {
            Console.WriteLine($"GetContentsByViewComponent VisitSN: {VisitSN}");

            return ViewComponent("VC_Comment", new { VisitSN = VisitSN });
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", comments.MemberID);
            //ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
            ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN", comments.SN);
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
                    if (!CommentsExists(comments.CommentID))
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

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(c => c.Member)
                //.Include(c => c.ParentComment)
                .Include(c => c.Visit)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comments == null)
            {
                return NotFound();
            }

            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments != null)
            {
                _context.Comments.Remove(comments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentsExists(string id)
        {
            return _context.Comments.Any(e => e.CommentID == id);
        }
    }
}
