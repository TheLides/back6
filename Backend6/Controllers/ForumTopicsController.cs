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
    public class ForumTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager1;
        private readonly IUserPermissionsService userPermissionsService;

        public ForumTopicsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _context = context; 
            this.userManager1 = userManager;
            this.userPermissionsService = userPermissions;
        }

        // GET: ForumTopics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ForumTopics.Include(f => f.Creator).Include(f => f.Forum);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        // GET: ForumTopics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .Include(f => f.ForumMessages)
                .ThenInclude(f => f.Creator)
                .Include(f => f.ForumMessages)
                .ThenInclude(f => f.ForumMessageAttachments)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // GET: ForumTopics/Create
        public async Task<IActionResult> Create(Guid? forumId)
        {
            if (forumId == null)
            {
                return this.NotFound();
            }

            var forum = await this._context.Forums
               .SingleOrDefaultAsync(x => x.Id == forumId);

            if (forum == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Forum = forum;
            return View(new ForumTopicViewModel());
        }

        // POST: ForumTopics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? forumId, ForumTopicViewModel model)
        {
            if (forumId == null)
            {
                return NotFound();
            }

            var forum = await this._context.Forums
               .SingleOrDefaultAsync(x => x.Id == forumId);

            if (forum == null)
            {
                return NotFound();
            }

            var user = await this.userManager1.GetUserAsync(this.HttpContext.User);

            if (ModelState.IsValid)
            {
                var time = DateTime.UtcNow;
                var forumTopic = new ForumTopic
                {
                    ForumId = forum.Id,
                    CreatorId = user.Id,
                    Created = time,
                    Name = model.Name
                };

                _context.Add(forumTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Forums", new { id = forum.Id });
            }

            ViewBag.Forum = forum;
            return View(model);
        }

        // GET: ForumTopics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(m => m.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (forumTopic == null || !this.userPermissionsService.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }

            var model = new ForumTopicViewModel
            {
                Name = forumTopic.Name
            };

            ViewBag.Forum = forumTopic.Forum;
            return View(model);
        }

        // POST: ForumTopics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, ForumTopicViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(m => m.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (forumTopic == null || !this.userPermissionsService.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forumTopic.Name = model.Name;

                await this._context.SaveChangesAsync();
                return RedirectToAction("Details", "Forums", new { id = forumTopic.ForumId });
            }

            ViewBag.Forum = forumTopic.Forum;
            return View(model);
        }

        // GET: ForumTopics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null || !this.userPermissionsService.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }
            ViewBag.Forum = forumTopic.Forum;
            return View(forumTopic);
        }

        // POST: ForumTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null || !this.userPermissionsService.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }
            ViewBag.Forum = forumTopic.Forum;
            _context.ForumTopics.Remove(forumTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Forums", new { id = forumTopic.ForumId });
        }
    }
}
