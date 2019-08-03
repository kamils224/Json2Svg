using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IeasteJson2Svg.Models;
using IeasteJson2Svg.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IeasteJson2Svg.Controllers
{
    public class DocumentsGeneratorController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public DocumentsGeneratorController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            SvgDocumentEditModel model = new SvgDocumentEditModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateDocument(string input)
        {
            Dictionary<string, string[]> jsonInput;
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
            string path = directory + "test.svg";

            int max = jsonInput.First().Value.Length;
            int index = 0;
            List<ZipItem> outputFiles = new List<ZipItem>();

            foreach (var item in jsonInput)
            {
                int currentLength = item.Value.Length;
                if (currentLength != max)
                {
                    ViewData["Error"] = "Value array error in column: " + item.Key + ". Expected length: " + max
                        + ", current length: " + currentLength;
                    return View();
                }

                SvgDocumentEditModel model = new SvgDocumentEditModel()
                {
                    TemplateDocumentPath = path,
                    ElementName = "text",
                    Attribute = "id",
                    ValueIndex = index,
                    ElementsForSubstitution = jsonInput
                };
                index++;
                ZipItem zipItem = new ZipItem("Document" + index+".svg", SvgEditor.GenerateSvgDocument(model));
                outputFiles.Add(zipItem);
            }

            var resultZip = Zipper.Zip(outputFiles);

            return File(resultZip, "application/octet-stream","Documents.zip");
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
                {".csv", "text/csv"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
            };
        }
    }
}