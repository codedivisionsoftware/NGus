using System;

namespace NGus.Models
{
    public class EntityModel
    {
        public string NationalBusinessRegistryNumber { get; set; }
        public string TaxpayerIdentificationNumber { get; set; }
        public string TaxpayerIdentificationNumberStatus { get; set; }
        public string Name { get; set; }
        public string Voivodeship { get; set; }
        public string District { get; set; }
        public string Commune { get; set; }
        public string Locality { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string PropertyNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string Type { get; set; }
        public int? SilosId { get; set; }
        public DateTime? TerminationActivityDate { get; set; }
        public string PostOfficeLocation { get; set; }
    }
}