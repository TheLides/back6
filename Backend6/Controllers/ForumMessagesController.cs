using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Backend6.Services;
using Backend6.Models.ViewModels;

namespace Backend6.Controllers
{
    [Authorize]
    public class ForumMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissionsService;

        public ForumMessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            this.userManager = userManager;
            this.userPermissionsService = userPermissions;

        }

        // GET: ForumMessages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ForumMessages.Include(f => f.Creator).Include(f => f.ForumTopic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ForumMessages/Create
        public async Task<IActionResult> Create(Guid? topicId)
        {
            if (topicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(m => m.Id == topicId);

            if (topic == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Topic = topic;
            return View(new ForumMessageViewModel());
        }

        // POST: ForumMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid topicId, ForumMessageViewModel model)
        {
            if (topicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(m => m.Id == topicId);

            if (topic == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (ModelState.IsValid)
            {
                var time = DateTime.UtcNow;
                var forumMessage = new ForumMessage
                {
                    ForumTopicId = topic.Id,
                    CreatorId = user.Id,
                    Created = time,
                    Modified = time,
                    Text = model.Text
                };
                _context.Add(forumMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ForumTopics", new { id = topic.Id });
            }
            ViewBag.Topic = topic;
            return View(model);
        }

        // GET: ForumMessages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null || !this.userPermissionsService.CanEditForumMessage(forumMessage))
            {
                return NotFound();
            }

            var model = new ForumMessageViewModel
            {
                Text = forumMessage.Text
            };
            this.ViewBag.Topic = forumMessage.ForumTopic;
            return View(model);
        }

        // POST: ForumMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, ForumMessageViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null || !this.userPermissionsService.CanEditForumMessage(forumMessage))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forumMessage.Text = model.Text;
                forumMessage.Modified = DateTime.UtcNow;

                await this._context.SaveChangesAsync();
                return RedirectToAction("Details", "ForumTopics", new { id = forumMessage.ForumTopicId });
            }
            this.ViewBag.Topic = forumMessage.ForumTopic;
            return View(forumMessage);
        }

        // GET: ForumMessages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.Creator)
                .Include(f => f.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }
            this.ViewBag.Topic = forumMessage.ForumTopic;
            return View(forumMessage);
        }

        // POST: ForumMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.Creator)
                .Include(f => f.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }
            this.ViewBag.Topic = forumMessage.ForumTopic;
            _context.ForumMessages.Remove(forumMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ForumTopics", new { id = forumMessage.ForumTopicId });
        }
    }
}
