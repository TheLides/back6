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
using System.Collections.ObjectModel;

namespace Backend6.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class ForumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Forums
        public async Task<IActionResult> Index(Guid? categoryId)
        {
            return View(await _context.Forums.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Forums/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(x => x.ForumTopics)
                .ThenInclude(p => p.Creator)
                .Include(f => f.ForumTopics)
                .ThenInclude(m => m.ForumMessages)
                .ThenInclude(p => p.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            this.ViewBag.Forum = forum;
            return View(forum);
        }

        // GET: Forums/Create
        public async Task<IActionResult> Create(Guid? categoryId)
        {
            if (categoryId == null)
            {
                return this.NotFound();
            }

            var category = await this._context.ForumCategories
                .SingleOrDefaultAsync(x => x.Id == categoryId);

            if (category == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Category = category;
            return this.View(new ForumCreateViewModel());

        }

        // POST: Forums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? categoryId, ForumCreateViewModel model)
        {
            if (categoryId == null)
            {
                return this.NotFound();
            }

            var category = await this._context.ForumCategories
                .SingleOrDefaultAsync(x => x.Id == categoryId);

            if (category == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var forum = new Forum
                {
                    ForumCategoryId = category.Id,
                    Name = model.Name,
                    Description = model.Description,
                    ForumTopics = new Collection<ForumTopic>()
                };
                _context.Forums.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ForumCategories");
            }
            this.ViewBag.Category = category;
            return View(model);
        }

        // GET: Forums/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(x => x.ForumCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (forum == null)
            {
                return NotFound();
            }

            var model = new ForumCreateViewModel
            {
                Name = forum.Name,
                Description = forum.Description
            };

            this.ViewBag.Forum = forum;
            return View(model);
        }

        // POST: Forums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, ForumCreateViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(x => x.ForumCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (forum == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forum.Name = model.Name;
                forum.Description = model.Description;
                return RedirectToAction("Index", "ForumCategories");
            }
            this.ViewBag.Forum = forum;
            return View(model);
        }

        // GET: Forums/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(x => x.ForumCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            this.ViewBag.Forum = forum;
            return View(forum);
        }

        // POST: Forums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
            this.ViewBag.Forum = forum;
            return RedirectToAction("Index", "ForumCategories");
        }
    }
}
