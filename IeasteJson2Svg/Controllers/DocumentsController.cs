using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using IeasteJson2Svg.Models;
using IeasteJson2Svg.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IeasteJson2Svg.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public DocumentsController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DocumentGenerator()
        {
            SvgDocumentEditModel model = new SvgDocumentEditModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult JsonTest(string JsonData)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DocumentGenerator(string input)
        {
            Dictionary<string, string> jsonInput;
            try
            {
                jsonInput = JsonExtractor.GetJsonData(input);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View();
            }


            string directory = _hostingEnvironment.WebRootPath + "/documents/";
            string path = directory+"test.svg";
            string outputFileName = "result.svg";

            SvgDocumentEditModel model = new SvgDocumentEditModel()
            {
                Filename = path,
                OutputFilename = directory + outputFileName,
                ElementName = "text",
                Attribute = "id",
                ElementsForSubstitution = jsonInput
            };

            var memory = await SvgEditor.SetDocumentElements(model);

            memory.Position = 0;

            return File(memory, GetContentType(path), outputFileName);
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".svg", "text/svg"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
            };
        }
    }
}