using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NGus.Lib.Models;

namespace NGus.Lib.Helpers
{
    internal static class NationalCourtRegisterHelper
    {
        public static IEnumerable<NationalCourtRegisterModel> Parse(string xml)
        {
            if (string.IsNullOrEmpty(xml) || string.IsNullOrWhiteSpace(xml))
            {
                return null;
            }

            XDocument xDocument;

            try
            {
                xDocument = XDocument.Load(xml);
            }
            catch
            {
                return null;
            }

            var nationalCourtRegisters = new List<NationalCourtRegisterModel>();

            var xRoot = xDocument.Root;
            var xData = xRoot?.Elements("dane").ToList();

            if (xData != null && xData.Any())
            {
                foreach (var xSingleData in xData)
                {
                    var xName = xSingleData?.Element("Nazwa");
                    var name = !string.IsNullOrEmpty(xName?.Value) && !string.IsNullOrWhiteSpace(xName.Value) ? xName.Value : null;

                    nationalCourtRegisters.Add(new NationalCourtRegisterModel
                    {
                        Name = name
                    });
                }
            }

            return nationalCourtRegisters;
        }
    }
}