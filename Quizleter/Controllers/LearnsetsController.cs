﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quizleter.Data;
using Quizleter.Models;

namespace Quizleter.Controllers
{
    public class LearnsetsController : Controller
    {
        private readonly QuizleterContext _context;

        public LearnsetsController(QuizleterContext context)
        {
            _context = context;
        }

        // GET: Learnsets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Learnset.ToListAsync());
        }

        // GET: Learnsets/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnset = await _context.Learnset
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learnset == null)
            {
                return NotFound();
            }

            return View(learnset);
        }

        // GET: Learnsets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Learnsets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Desc")] Learnset learnset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learnset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learnset);
        }

        // GET: Learnsets/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnset = await _context.Learnset.FindAsync(id);
            if (learnset == null)
            {
                return NotFound();
            }
            return View(learnset);
        }

        // POST: Learnsets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Desc")] Learnset learnset)
        {
            if (id != learnset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learnset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnsetExists(learnset.Id))
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
            return View(learnset);
        }

        // GET: Learnsets/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnset = await _context.Learnset
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learnset == null)
            {
                return NotFound();
            }

            return View(learnset);
        }

        // POST: Learnsets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var learnset = await _context.Learnset.FindAsync(id);
            _context.Learnset.Remove(learnset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnsetExists(long id)
        {
            return _context.Learnset.Any(e => e.Id == id);
        }
    }
}
