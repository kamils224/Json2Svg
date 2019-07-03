using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IeasteJson2Svg;

namespace IeasteJson2Svg.Models
{
    public class SvgDocumentsController : Controller
    {
        private readonly DocumentsContainerContext _context;

        public SvgDocumentsController(DocumentsContainerContext context)
        {
            _context = context;
        }

        // GET: SvgDocuments
        public async Task<IActionResult> Index()
        {
            return View(await _context.SvgDocuments.ToListAsync());
        }

        // GET: SvgDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var svgDocument = await _context.SvgDocuments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (svgDocument == null)
            {
                return NotFound();
            }

            return View(svgDocument);
        }

        // GET: SvgDocuments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SvgDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DocumentName,DocumentPath")] SvgDocument svgDocument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(svgDocument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(svgDocument);
        }

        // GET: SvgDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var svgDocument = await _context.SvgDocuments.FindAsync(id);
            if (svgDocument == null)
            {
                return NotFound();
            }
            return View(svgDocument);
        }

        // POST: SvgDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DocumentName,DocumentPath")] SvgDocument svgDocument)
        {
            if (id != svgDocument.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(svgDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SvgDocumentExists(svgDocument.ID))
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
            return View(svgDocument);
        }

        // GET: SvgDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var svgDocument = await _context.SvgDocuments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (svgDocument == null)
            {
                return NotFound();
            }

            return View(svgDocument);
        }

        // POST: SvgDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var svgDocument = await _context.SvgDocuments.FindAsync(id);
            _context.SvgDocuments.Remove(svgDocument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SvgDocumentExists(int id)
        {
            return _context.SvgDocuments.Any(e => e.ID == id);
        }
    }
}
