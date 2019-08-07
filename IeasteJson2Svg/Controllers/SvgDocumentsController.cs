using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IeasteJson2Svg;
using IeasteJson2Svg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using IeasteJson2Svg.Tools;
using System.Xml;

namespace IeasteJson2Svg.Controllers
{
    public class SvgDocumentsController : Controller
    {
        private readonly DocumentsContainerContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SvgDocumentsController(DocumentsContainerContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

            string pathToFile = _hostingEnvironment.WebRootPath + svgDocument.DocumentPath;
            XmlDocument doc = new XmlDocument();
            doc.Load(pathToFile);
            var editableElements = _context.SvgElements.Where(x => x.DocumentId == id).OrderByDescending(x => x.IsActive).ToList();

            ViewData["Elements"] = editableElements;

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
        public async Task<IActionResult> Create([Bind("ID, DocumentName, Description")] SvgDocument svgDocument, IFormFile file)
        {
            if(file!=null)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".svg")
                {
                    throw new FormatException("Invalid file extension!");
                }
                string localPath = "/documents/" + file.FileName;
                string globalPath = _hostingEnvironment.WebRootPath + localPath; 
                svgDocument.DocumentPath = localPath;

                if (ModelState.IsValid)
                {
                    _context.Add(svgDocument);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return View(svgDocument);
                }

                using (FileStream fs = new FileStream(globalPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(globalPath);
                var editableElements = SvgEditor.FindEditableElements(doc, "text", "id");

                for (int i = 0; i < editableElements.Count; i++)
                {
                    SvgElement element = new SvgElement()
                    {
                        DocumentId = svgDocument.ID,
                        AttributeName = editableElements[i].Attributes["id"].Value,
                        AttributeInnerText = editableElements[i].InnerText,
                        IsActive = true
                    };
                    _context.SvgElements.Add(element);
                }
                _context.SaveChanges();

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
        public async Task<IActionResult> Edit(int id, [Bind("ID,DocumentName, Description")] SvgDocument svgDocument)
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

            _context.SvgElements.RemoveRange(_context.SvgElements.Where(el => el.DocumentId == id));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SvgDocumentExists(int id)
        {
            return _context.SvgDocuments.Any(e => e.ID == id);
        }
    }
}
