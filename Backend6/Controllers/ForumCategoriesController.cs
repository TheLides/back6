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
using Backend6.Models.ViewModels;

namespace Backend6.Controllers
{
    
    public class ForumCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ForumCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ForumCategories
                .Include(x => x.Forums)
                .ThenInclude(x => x.ForumTopics)
                .ToListAsync());
        }

        [Authorize(Roles = ApplicationRoles.Administrators)]
        // GET: ForumCategories/Create
        public IActionResult Create()
        {
            return View(new ForumCategoryViewModel());
        }

        // POST: ForumCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = ApplicationRoles.Administrators)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var forumCategory = new ForumCategory
                {
                    Name = model.Name
                };

                _context.ForumCategories.Add(forumCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ForumCategories/Edit/5

        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            var model = new ForumCategoryViewModel
            {
                Name = forumCategory.Name
            };

            return View(model);
        }

        // POST: ForumCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = ApplicationRoles.Administrators)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumCategoryViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forumCategory.Name = model.Name;
                await this._context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ForumCategories/Delete/5
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // POST: ForumCategories/Delete/5
        [Authorize(Roles = ApplicationRoles.Administrators)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumCategories.Remove(forumCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
