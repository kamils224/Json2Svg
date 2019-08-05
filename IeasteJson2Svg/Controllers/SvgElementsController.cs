using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IeasteJson2Svg;
using IeasteJson2Svg.Models;

namespace IeasteJson2Svg.Controllers
{
    public class SvgElementsController : Controller
    {
        private readonly DocumentsContainerContext _context;

        public SvgElementsController(DocumentsContainerContext context)
        {
            _context = context;
        }

        // GET: SvgElements
        public async Task<IActionResult> Index()
        {
            return View(await _context.SvgElements.ToListAsync());
        }

        // GET: SvgElements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var svgElement = await _context.SvgElements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (svgElement == null)
            {
                return NotFound();
            }

            return View(svgElement);
        }

        // GET: SvgElements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SvgElements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DocumentId,AttributeName,AttributeInnerText,IsActive")] SvgElement svgElement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(svgElement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(svgElement);
        }

        // GET: SvgElements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var svgElement = await _context.SvgElements.FindAsync(id);
            if (svgElement == null)
            {
                return NotFound();
            }
            return View(svgElement);
        }

        // POST: SvgElements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DocumentId,AttributeName,AttributeInnerText,IsActive")] SvgElement svgElement)
        {
            if (id != svgElement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(svgElement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SvgElementExists(svgElement.Id))
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
            return View(svgElement);
        }

        // GET: SvgElements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var svgElement = await _context.SvgElements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (svgElement == null)
            {
                return NotFound();
            }

            return View(svgElement);
        }

        // POST: SvgElements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var svgElement = await _context.SvgElements.FindAsync(id);
            _context.SvgElements.Remove(svgElement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SvgElementExists(int id)
        {
            return _context.SvgElements.Any(e => e.Id == id);
        }
    }
}
