using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using InvoicePro.Api.Services.UslugaBIRzewnPubl;
using WcfCoreMtomEncoder;

namespace NGus
{
    public class NGus
    {
        private readonly string _userKey;

        private UslugaBIRzewnPublClient _client;
        private ZalogujResponse _signInResponse;

        public NGus(string userKey)
        {
            _userKey = userKey;
            InitializeClient();
        }

        public async Task<string> GetDataFromNationalCourtRegister(string nationalCourtRegister)
        {
            if (string.IsNullOrEmpty(nationalCourtRegister) || string.IsNullOrWhiteSpace(nationalCourtRegister))
            {
                return null;
            }

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
                    var dataSearchEntitiesResponse = await _client.DaneSzukajPodmiotyAsync(new DaneSzukajPodmiotyRequest(new ParametryWyszukiwania
                    {
                        Krs = nationalCourtRegister
                    }));

                    xml = dataSearchEntitiesResponse?.DaneSzukajPodmiotyResult;
                }
            }

            if (!string.IsNullOrEmpty(xml) && !string.IsNullOrWhiteSpace(xml))
            {

            }

            await SignOut();

            return xml;
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