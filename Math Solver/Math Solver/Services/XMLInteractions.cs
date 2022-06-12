using Math_Solver.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static Math_Solver.App;

namespace Math_Solver.Services
{
    class XMLInteractions
    {
        public void GetXmlGroup()
        {
                var assembly = typeof(InitPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("Math_Solver.Resources.formulaXML.xml");

                XDocument xDoc;
                xDoc = XDocument.Load(stream);

                var result = from r in xDoc.Descendants("Formula")
                             where (r.Element("Tag").Value == "Group")
                             select new
                             {
                                 Id = int.Parse(r.Element("Id").Value),
                                 Name = r.Element("Name").Value,
                                 Tag = r.Element("Tag").Value,
                             };

        }
    }
}
