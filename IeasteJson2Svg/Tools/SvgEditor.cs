﻿using IeasteJson2Svg.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace IeasteJson2Svg.Tools
{
    public class SvgEditor
    {
        public static async Task<MemoryStream> SetDocumentElements(SvgDocumentEditModel model)
        {
            string tag = model.ElementName;
            string attribute = model.Attribute;

            XmlDocument doc = new XmlDocument();
            doc.Load(model.Filename);

            var elementList = doc.GetElementsByTagName(tag);

            for (int i = 0; i < elementList.Count; i++)
            {
                string idValue = elementList[i].Attributes[attribute].Value;
                if (model.ElementsForSubstitution.ContainsKey(idValue))
                {
                    elementList[i].InnerText = model.ElementsForSubstitution[idValue];
                }
            }
            var memory = new MemoryStream();

            doc.Save(memory);
            memory.Flush();
            memory.Position = 0;
            return memory;
        }
    }
}