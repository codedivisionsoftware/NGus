using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using InvoicePro.Api.Services.UslugaBIRzewnPubl;
using NGus.Enumerations;
using NGus.Helpers;
using NGus.Models;
using WcfCoreMtomEncoder;

namespace NGus.Contexts
{
    public class NGusContext
    {
        private readonly string _userKey;

        private UslugaBIRzewnPublClient _client;
        private ZalogujResponse _signInResponse;

        public NGusContext(string userKey)
        {
            _userKey = userKey;
            InitializeClient();
        }

        public async Task<EntityModel> GetEntityByTaxpayerIdentificationNumber(string taxpayerIdentificationNumber)
        {
            return await GetEntity(IdentifierType.TaxpayerIdentificationNumber, taxpayerIdentificationNumber);
        }

        public async Task<EntityModel> GetEntityByNationalBusinessRegistryNumber(string nationalBusinessRegistryNumber)
        {
            return await GetEntity(IdentifierType.NationalBusinessRegistryNumber, nationalBusinessRegistryNumber);
        }

        public async Task<EntityModel> GetEntityByNationalBusinessRegistryNumber9Characters(string nationalBusinessRegistryNumber9Characters)
        {
            return await GetEntity(IdentifierType.NationalBusinessRegistryNumber9Characters, nationalBusinessRegistryNumber9Characters);
        }

        public async Task<EntityModel> GetEntityByNationalBusinessRegistryNumber14Characters(string nationalBusinessRegistryNumber14Characters)
        {
            return await GetEntity(IdentifierType.NationalBusinessRegistryNumber14Characters, nationalBusinessRegistryNumber14Characters);
        }

        public async Task<EntityModel> GetEntityByNationalCourtRegister(string nationalCourtRegister)
        {
            return await GetEntity(IdentifierType.NationalCourtRegister, nationalCourtRegister);
        }

        public async Task<EntityModel> GetEntityByTaxpayerIdentificationNumbers(IEnumerable<string> taxpayerIdentificationNumbers)
        {
            return await GetEntity(IdentifierType.TaxpayerIdentificationNumber, taxpayerIdentificationNumbers);
        }

        public async Task<EntityModel> GetEntityByNationalBusinessRegistryNumbers(IEnumerable<string> nationalBusinessRegistryNumbers)
        {
            return await GetEntity(IdentifierType.NationalBusinessRegistryNumber, nationalBusinessRegistryNumbers);
        }

        public async Task<EntityModel> GetEntityByNationalBusinessRegistryNumbers9Characters(IEnumerable<string> nationalBusinessRegistryNumbers9Characters)
        {
            return await GetEntity(IdentifierType.NationalBusinessRegistryNumber9Characters, nationalBusinessRegistryNumbers9Characters);
        }

        public async Task<EntityModel> GetEntityByNationalBusinessRegistryNumbers14Characters(IEnumerable<string> nationalBusinessRegistryNumbers14Characters)
        {
            return await GetEntity(IdentifierType.NationalBusinessRegistryNumber14Characters, nationalBusinessRegistryNumbers14Characters);
        }

        public async Task<EntityModel> GetEntityByNationalCourtRegisters(IEnumerable<string> nationalCourtRegisters)
        {
            return await GetEntity(IdentifierType.NationalCourtRegister, nationalCourtRegisters);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByTaxpayerIdentificationNumber(string taxpayerIdentificationNumber)
        {
            return await GetEntities(IdentifierType.TaxpayerIdentificationNumber, taxpayerIdentificationNumber);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalBusinessRegistryNumber(string nationalBusinessRegistryNumber)
        {
            return await GetEntities(IdentifierType.NationalBusinessRegistryNumber, nationalBusinessRegistryNumber);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalBusinessRegistryNumber9Characters(string nationalBusinessRegistryNumber9Characters)
        {
            return await GetEntities(IdentifierType.NationalBusinessRegistryNumber9Characters, nationalBusinessRegistryNumber9Characters);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalBusinessRegistryNumber14Characters(string nationalBusinessRegistryNumber14Characters)
        {
            return await GetEntities(IdentifierType.NationalBusinessRegistryNumber14Characters, nationalBusinessRegistryNumber14Characters);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalCourtRegister(string nationalCourtRegister)
        {
            return await GetEntities(IdentifierType.NationalCourtRegister, nationalCourtRegister);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByTaxpayerIdentificationNumbers(IEnumerable<string> taxpayerIdentificationNumbers)
        {
            return await GetEntities(IdentifierType.TaxpayerIdentificationNumber, taxpayerIdentificationNumbers);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalBusinessRegistryNumbers(IEnumerable<string> nationalBusinessRegistryNumbers)
        {
            return await GetEntities(IdentifierType.NationalBusinessRegistryNumber, nationalBusinessRegistryNumbers);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalBusinessRegistryNumbers9Characters(IEnumerable<string> nationalBusinessRegistryNumbers9Characters)
        {
            return await GetEntities(IdentifierType.NationalBusinessRegistryNumber9Characters, nationalBusinessRegistryNumbers9Characters);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalBusinessRegistryNumbers14Characters(IEnumerable<string> nationalBusinessRegistryNumbers14Characters)
        {
            return await GetEntities(IdentifierType.NationalBusinessRegistryNumber14Characters, nationalBusinessRegistryNumbers14Characters);
        }

        public async Task<IEnumerable<EntityModel>> GetEntitiesByNationalCourtRegisters(IEnumerable<string> nationalCourtRegisters)
        {
            return await GetEntities(IdentifierType.NationalCourtRegister, nationalCourtRegisters);
        }

        public async Task<EntityModel> GetEntity(IdentifierType identifierType, string identifier)
        {
            return (await GetEntities(identifierType, identifier)).FirstOrDefault();
        }

        public async Task<EntityModel> GetEntity(IdentifierType identifierType, IEnumerable<string> identifiers)
        {
            return (await GetEntities(identifierType, identifiers)).FirstOrDefault();
        }

        public async Task<IEnumerable<EntityModel>> GetEntities(IdentifierType identifierType, string identifier)
        {
            await SignIn();
            string xml = null;

            using (new OperationContextScope(_client?.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty
                {
                    Headers =
                    {
                        { "sid", _signInResponse?.ZalogujResult }
                    }
                };

                if (_client != null)
                {
                    var dataSearchEntitiesResponse = await SearchEntity(identifierType, identifier);
                    xml = dataSearchEntitiesResponse?.DaneSzukajPodmiotyResult;
                }
            }

            await SignOut();
            return EntityHelper.Parse(xml);
        }

        public async Task<IEnumerable<EntityModel>> GetEntities(IdentifierType identifierType, IEnumerable<string> identifiers)
        {
            await SignIn();
            string xml = null;

            using (new OperationContextScope(_client?.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty
                {
                    Headers =
                    {
                        { "sid", _signInResponse?.ZalogujResult }
                    }
                };

                if (_client != null)
                {
                    var dataSearchEntitiesResponse = await SearchEntity(identifierType, identifiers);
                    xml = dataSearchEntitiesResponse?.DaneSzukajPodmiotyResult;
                }
            }

            await SignOut();
            return EntityHelper.Parse(xml);
        }

        private void InitializeClient()
        {
            _client = new UslugaBIRzewnPublClient
            {
                Endpoint =
                {
                    Binding = new CustomBinding(new MtomMessageEncoderBindingElement(new TextMessageEncodingBindingElement()), new HttpsTransportBindingElement())
                }
            };
        }

        private async Task SignIn()
        {
            if (_client == null || string.IsNullOrEmpty(_userKey) || string.IsNullOrWhiteSpace(_userKey))
            {
                return;
            }

            _signInResponse = await _client.ZalogujAsync(new ZalogujRequest(_userKey));
        }

        private async Task<DaneSzukajPodmiotyResponse> SearchEntity(IdentifierType identifierType, string identifier)
        {
            switch (identifierType)
            {
                case IdentifierType.TaxpayerIdentificationNumber:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Nip = identifier
                    }));

                case IdentifierType.NationalBusinessRegistryNumber:
                case IdentifierType.NationalBusinessRegistryNumber9Characters:
                case IdentifierType.NationalBusinessRegistryNumber14Characters:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Regon = identifier
                    }));

                case IdentifierType.NationalCourtRegister:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Krs = identifier
                    }));

                default:
                    return null;
            }
        }

        private async Task<DaneSzukajPodmiotyResponse> SearchEntity(IdentifierType identifierType, IEnumerable<string> identifiers)
        {
            identifiers = identifiers?.ToList();

            if (identifiers == null || !identifiers.Any())
            {
                return null;
            }

            switch (identifierType)
            {
                case IdentifierType.TaxpayerIdentificationNumber:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Nipy = string.Join(",", identifiers)
                    }));

                case IdentifierType.NationalBusinessRegistryNumber9Characters:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Regony9zn = string.Join(",", identifiers)
                    }));

                case IdentifierType.NationalBusinessRegistryNumber14Characters:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Regony14zn = string.Join(",", identifiers)
                    }));

                case IdentifierType.NationalCourtRegister:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Krsy = string.Join(",", identifiers)
                    }));

                default:
                    return null;
            }
        }

        private async Task SignOut()
        {
            if (_client == null || _signInResponse == null)
            {
                return;
            }

            await _client.WylogujAsync(new WylogujRequest(_signInResponse.ZalogujResult));
        }
    }
}