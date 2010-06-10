﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Orchard.ContentManagement.MetaData;
using Orchard.DevTools.ViewModels;

namespace Orchard.DevTools.Controllers {
    [ValidateInput(false)]
    public class MetadataController : Controller {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentDefinitionWriter _contentDefinitionWriter;
        private readonly IContentDefinitionReader _contentDefinitionReader;

        public MetadataController(
            IContentDefinitionManager contentDefinitionManager,
            IContentDefinitionWriter contentDefinitionWriter,
            IContentDefinitionReader contentDefinitionReader) {
            _contentDefinitionManager = contentDefinitionManager;
            _contentDefinitionWriter = contentDefinitionWriter;
            _contentDefinitionReader = contentDefinitionReader;
        }

        public ActionResult Index() {
            var model = new MetadataIndexViewModel {
                TypeDefinitions = _contentDefinitionManager.ListTypeDefinitions(),
                PartDefinitions = _contentDefinitionManager.ListPartDefinitions()
            };
            var types = new XElement("Types");
            foreach (var type in model.TypeDefinitions) {
                types.Add(_contentDefinitionWriter.Export(type));
            }

            var parts = new XElement("Parts");
            foreach (var part in model.PartDefinitions) {
                parts.Add(_contentDefinitionWriter.Export(part));
            }

            var stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, IndentChars = "  " })) {
                if (xmlWriter != null) {
                    new XElement("Orchard", types, parts).WriteTo(xmlWriter);
                }
            }
            model.ExportText = stringWriter.ToString();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(MetadataIndexViewModel model) {
            var root = XElement.Parse(model.ExportText);
            foreach (var element in root.Elements("Types").Elements()) {
                var typeElement = element;
                var typeName = XmlConvert.DecodeName(element.Name.LocalName);
                _contentDefinitionManager.AlterTypeDefinition(typeName, alteration => _contentDefinitionReader.Merge(typeElement, alteration));
            }
            return RedirectToAction("Index");
        }
    }
}