using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AniwalkServer.Controllers
{
    [Authorize(Roles = Shared.Role_Member)]
    public class RepliesController : Controller
    {
        private readonly AniwalkDBContext _context;

        public RepliesController(AniwalkDBContext context)
        {
            _context = context;
        }

        // GET: Replies/Details/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="CommentID"></param>
        /// <param name="ParentReplyID">要回覆哪個留言</param>
        /// <returns></returns>
        public IActionResult Create(int VisitSN, string CommentID, string? ParentReplyID)
        {
            //Console.WriteLine($"Replies Create VisitSN : {VisitSN}, CommentID : {CommentID}, ParentReplyID : {ParentReplyID}");

            ViewData["CommentID"] = CommentID;
            ViewData["ParentReplyID"] = ParentReplyID;
            ViewData["SN"] = VisitSN;

            return View();
        }

        // POST: Replies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReplyID,ReplyContent,CommentID,ParentReplyID,MemberID,SN")] Replies replies)
        {
            if (ModelState.IsValid)
            {
                replies.ReplyDate = DateTime.Now;

                _context.Add(replies);
                await _context.SaveChangesAsync();
                return Json(replies);
            }

            //Shared.ShowModelState(ModelState);

            //ViewData["CommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", replies.CommentID);
            //ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", replies.MemberID);
            //ViewData["ParentReplyID"] = new SelectList(_context.Replies, "ReplyID", "ReplyID", replies.ParentReplyID);
            return Json(replies);
        }

        // GET: Replies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var replies = await _context.Replies.FindAsync(id);
            if (replies == null)
            {
                return NotFound();
            }
            ViewData["CommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", replies.CommentID);
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", replies.MemberID);
            ViewData["ParentReplyID"] = new SelectList(_context.Replies, "ReplyID", "ReplyID", replies.ParentReplyID);
            return View(replies);
        }

        // POST: Replies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ReplyID,ReplyContent,ReplyDate,CommentID,ParentReplyID,MemberID")] Replies replies)
        {
            if (id != replies.ReplyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(replies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepliesExists(replies.ReplyID))
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
            ViewData["CommentID"] = new SelectList(_context.Comments, "CommentID", "CommentID", replies.CommentID);
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", replies.MemberID);
            ViewData["ParentReplyID"] = new SelectList(_context.Replies, "ReplyID", "ReplyID", replies.ParentReplyID);
            return View(replies);
        }

        // GET: Replies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var replies = await _context.Replies
                .Include(r => r.Comment)
                .Include(r => r.Member)
                .Include(r => r.ParentReply)
                .FirstOrDefaultAsync(m => m.ReplyID == id);
            if (replies == null)
            {
                return NotFound();
            }

            return View(replies);
        }

        // POST: Replies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var replies = await _context.Replies.FindAsync(id);
            if (replies != null)
            {
                _context.Replies.Remove(replies);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepliesExists(string id)
        {
            return _context.Replies.Any(e => e.ReplyID == id);
        }
    }
}
