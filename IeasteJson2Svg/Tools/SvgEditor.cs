using IeasteJson2Svg.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Svg;
using System.Drawing;
using System.Drawing.Imaging;

namespace IeasteJson2Svg.Tools
{
    public class SvgEditor
    {
        public static MemoryStream GenerateSvgDocument(SvgDocumentEditModel model)
        {
            
            string tag = model.ElementName;
            string attribute = model.Attribute;

            XmlDocument doc = new XmlDocument();
            doc.Load(model.TemplateDocumentPath);

            var elementList = doc.GetElementsByTagName(tag);

            for (int i = 0; i < elementList.Count; i++)
            {
                string idValue = elementList[i].Attributes[attribute].Value;
                if (model.ElementsForSubstitution.ContainsKey(idValue))
                {
                    elementList[i].ChildNodes[0].InnerText = model.ElementsForSubstitution[idValue][model.ValueIndex];
                }
            }
            var memory = new MemoryStream();

            doc.Save(memory);
            memory.Flush();
            memory.Position = 0;
            return memory;
        }

        public static List<XmlNode> FindEditableElements(XmlDocument doc,string tag, string attribute)
        {

            var elementList = doc.GetElementsByTagName(tag);
            List<XmlNode> resultList = new List<XmlNode>();
            for (int i = 0; i < elementList.Count; i++)
            {
                string idValue = elementList[i].Attributes[attribute].Value;
                if (!string.IsNullOrEmpty(idValue))
                {
                    resultList.Add(elementList[i]);
                }
            }

            return resultList;
        }

        public static MemoryStream SvgToJpeg(Stream svgDoc)
        {
            var document = Svg.SvgDocument.Open<Svg.SvgDocument>(svgDoc);
            document.ShapeRendering = SvgShapeRendering.Auto;
            Bitmap bmp = document.Draw(12,12);
            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Jpeg);
            return stream;
        }
    }
}
