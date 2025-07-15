using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;

namespace AniwalkServer.Controllers
{
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
        /// <param name="ParentCommentID">要回覆哪個留言</param>
        /// <returns></returns>
        public IActionResult Create(int VisitSN, string? ParentCommentID)
        {
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID");
            //ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID");
            ViewData["ParentCommentID"] = ParentCommentID;
            //ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN");
            ViewData["VisitSN"] = VisitSN;

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentID,CommentText,CommentDate,ParentCommentID,MemberID,SN")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", comments.MemberID);
            ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
            ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN", comments.SN);
            return View(comments);
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
            ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
            ViewData["SN"] = new SelectList(_context.Visits, "SN", "SN", comments.SN);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CommentID,CommentText,CommentDate,ParentCommentID,MemberID,SN")] Comments comments)
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
            ViewData["ParentCommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", comments.ParentCommentID);
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
                .Include(c => c.ParentComment)
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
