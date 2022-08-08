using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NGus.Models;

namespace NGus.Helpers
{
    internal static class EntityHelper
    {
        public static IEnumerable<EntityModel> Parse(string xml)
        {
            if (string.IsNullOrEmpty(xml) || string.IsNullOrWhiteSpace(xml))
            {
                return null;
            }

            XDocument xDocument;

            try
            {
                xDocument = XDocument.Parse(xml);
            }
            catch
            {
                return null;
            }

            var xRoot = xDocument.Root;
            var xData = xRoot?.Elements("dane").ToList();

            var entities = new List<EntityModel>();

            if (xData != null && xData.Any())
            {
                foreach (var xSingleData in xData)
                {
                    if (xSingleData == null)
                    {
                        continue;
                    }

                    var xNationalBusinessRegistryNumber = xSingleData?.Element("Regon");
                    var nationalBusinessRegistryNumber = !string.IsNullOrEmpty(xNationalBusinessRegistryNumber?.Value) && !string.IsNullOrWhiteSpace(xNationalBusinessRegistryNumber.Value) ? xNationalBusinessRegistryNumber.Value : null;

                    var xTaxpayerIdentificationNumber = xSingleData?.Element("Nip");
                    var taxpayerIdentificationNumber = !string.IsNullOrEmpty(xTaxpayerIdentificationNumber?.Value) && !string.IsNullOrWhiteSpace(xTaxpayerIdentificationNumber.Value) ? xTaxpayerIdentificationNumber.Value : null;

                    var xTaxpayerIdentificationNumberStatus = xSingleData?.Element("StatusNip");
                    var taxpayerIdentificationNumberStatus = !string.IsNullOrEmpty(xTaxpayerIdentificationNumberStatus?.Value) && !string.IsNullOrWhiteSpace(xTaxpayerIdentificationNumberStatus.Value) ? xTaxpayerIdentificationNumberStatus.Value : null;

                    var xName = xSingleData?.Element("Nazwa");
                    var name = !string.IsNullOrEmpty(xName?.Value) && !string.IsNullOrWhiteSpace(xName.Value) ? xName.Value : null;

                    var xVoivodeship = xSingleData?.Element("Wojewodztwo");
                    var voivodeship = !string.IsNullOrEmpty(xVoivodeship?.Value) && !string.IsNullOrWhiteSpace(xVoivodeship.Value) ? xVoivodeship.Value : null;

                    var xDistrict = xSingleData?.Element("Powiat");
                    var district = !string.IsNullOrEmpty(xDistrict?.Value) && !string.IsNullOrWhiteSpace(xDistrict.Value) ? xDistrict.Value : null;

                    var xCommune = xSingleData?.Element("Gmina");
                    var commune = !string.IsNullOrEmpty(xCommune?.Value) && !string.IsNullOrWhiteSpace(xCommune.Value) ? xCommune.Value : null;

                    var xLocality = xSingleData?.Element("Miejscowosc");
                    var locality = !string.IsNullOrEmpty(xLocality?.Value) && !string.IsNullOrWhiteSpace(xLocality.Value) ? xLocality.Value : null;

                    var xZipCode = xSingleData?.Element("KodPocztowy");
                    var zipCode = !string.IsNullOrEmpty(xZipCode?.Value) && !string.IsNullOrWhiteSpace(xZipCode.Value) ? xZipCode.Value : null;

                    var xStreet = xSingleData?.Element("Ulica");
                    var street = !string.IsNullOrEmpty(xStreet?.Value) && !string.IsNullOrWhiteSpace(xStreet.Value) ? xStreet.Value : null;

                    var xPropertyNumber = xSingleData?.Element("NrNieruchomosci");
                    var propertyNumber = !string.IsNullOrEmpty(xPropertyNumber?.Value) && !string.IsNullOrWhiteSpace(xPropertyNumber.Value) ? xPropertyNumber.Value : null;

                    var xApartmentNumber = xSingleData?.Element("NrLokalu");
                    var apartmentNumber = !string.IsNullOrEmpty(xApartmentNumber?.Value) && !string.IsNullOrWhiteSpace(xApartmentNumber.Value) ? xApartmentNumber.Value : null;

                    var xType = xSingleData?.Element("Typ");
                    var type = !string.IsNullOrEmpty(xType?.Value) && !string.IsNullOrWhiteSpace(xType.Value) ? xType.Value : null;

                    var xSilosId = xSingleData?.Element("SilosID");
                    var silosId = !string.IsNullOrEmpty(xSilosId?.Value) && !string.IsNullOrWhiteSpace(xSilosId.Value) && int.TryParse(xSilosId.Value, out _) ? (int?) Convert.ToInt32(xSilosId.Value) : null;

                    var xTerminationActivityDate = xSingleData?.Element("DataZakonczeniaDzialalnosci");
                    var terminationActivityDate = !string.IsNullOrEmpty(xTerminationActivityDate?.Value) && !string.IsNullOrWhiteSpace(xTerminationActivityDate.Value) && DateTime.TryParse(xTerminationActivityDate.Value, out _) ? (DateTime?) Convert.ToDateTime(xTerminationActivityDate.Value) : null;

                    var xPostOfficeLocation = xSingleData?.Element("MiejscowoscPoczty");
                    var postOfficeLocation = !string.IsNullOrEmpty(xPostOfficeLocation?.Value) && !string.IsNullOrWhiteSpace(xPostOfficeLocation.Value) ? xPostOfficeLocation.Value : null;

                    entities.Add(new EntityModel
                    {
                        NationalBusinessRegistryNumber = nationalBusinessRegistryNumber,
                        TaxpayerIdentificationNumber = taxpayerIdentificationNumber,
                        TaxpayerIdentificationNumberStatus = taxpayerIdentificationNumberStatus,
                        Name = name,
                        Voivodeship = voivodeship,
                        District = district,
                        Commune = commune,
                        Locality = locality,
                        ZipCode = zipCode,
                        Street = street,
                        PropertyNumber = propertyNumber,
                        ApartmentNumber = apartmentNumber,
                        Type = type,
                        SilosId = silosId,
                        TerminationActivityDate = terminationActivityDate,
                        PostOfficeLocation = postOfficeLocation
                    });
                }
            }

            return entities;
        }
    }
}