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
        private readonly DocumentsContainerContext _context;
        public DocumentsGeneratorController(DocumentsContainerContext context,IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (int.TryParse(id, out int docId))
            {
                SvgDocument document = _context.SvgDocuments.Where(x => x.ID == docId).FirstOrDefault();
                if (document != null)
                {
                    ViewData["DocumentDetails"] = document;
                }
                else
                {
                    return NotFound();
                }
                List<SvgElement> documentElements = _context.SvgElements
                    .Where(x => x.DocumentId == docId && x.IsActive).ToList();
                Dictionary<string, string[]> exampleJsonDict = new Dictionary<string, string[]>();
                int numOfExamples = 5;
                for (int i = 0; i < documentElements.Count; i++)
                {
                    string[] examples = new string[numOfExamples];
                    for (int j = 0; j < numOfExamples; j++)
                    {
                        examples[j] = "example" + j;
                    }
                    exampleJsonDict.Add(documentElements[i].AttributeName, examples);
                }

                ViewData["ExampleJson"] = JsonExtractor.GetJsonString(exampleJsonDict);
            }
            else
            {
                return NotFound();
            }

            SvgDocumentEditModel model = new SvgDocumentEditModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateDocument(string input, string docId)
        {
            Dictionary<string, string[]> jsonInput;
            try
            {
                jsonInput = JsonExtractor.GetJsonData(input);
            }
            catch (Exception e)
            {
                var error = e.Message;
                return Content(error);
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
                    var error = "Value array error in column: " + item.Key + ". Expected length: " + max
                        + ", current length: " + currentLength;
                    return Content(error);
                }
            }
            
            for (int i = 0; i < max; i++)
            {
                SvgDocumentEditModel model = new SvgDocumentEditModel()
                {
                    TemplateDocumentPath = path,
                    ElementName = "text",
                    Attribute = "id",
                    ValueIndex = index,
                    ElementsForSubstitution = jsonInput
                };
                index++;
                ZipItem zipItem = new ZipItem("Document" + index + ".svg", SvgEditor.GenerateSvgDocument(model));
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