using System.Collections.Generic;
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

        public async Task<IEnumerable<NationalCourtRegisterModel>> GetDataFromNationalCourtRegister(string nationalCourtRegister)
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
                    var dataSearchEntitiesResponse = await SearchEntity(EntityType.NationalCourtRegister, nationalCourtRegister);
                    xml = dataSearchEntitiesResponse?.DaneSzukajPodmiotyResult;
                }
            }

            await SignOut();
            return NationalCourtRegisterHelper.Parse(xml);
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

        private async Task<DaneSzukajPodmiotyResponse> SearchEntity(EntityType entityType, string value)
        {
            switch (entityType)
            {
                case EntityType.TaxpayerIdentificationNumber:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Nip = value
                    }));

                case EntityType.NationalBusinessRegistryNumber:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Regon = value
                    }));

                case EntityType.NationalCourtRegister:
                    return await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Krs = value
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