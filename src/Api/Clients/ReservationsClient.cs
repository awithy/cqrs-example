using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.Common;
using Api.DTOs;
using Newtonsoft.Json;

namespace Api.Clients
{
    public class ReservationsClientFactory
    {
        private readonly IApiConfig _config;

        public ReservationsClientFactory(IApiConfig config)
        {
            _config = config;
        }

        public ReservationsClient Create()
        {
            return new ReservationsClient(_config.BaseUri);
        }
    }

    public class ReservationsClient
    {
        private readonly Uri _baseUri;

        public ReservationsClient(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public async Task Create(CreateReservationRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var uri = new Uri(_baseUri, "api/v1/reservations");
                var content = JsonConvert.SerializeObject(request);
                var response = await httpClient.PostAsync(uri, new StringContent(content, Encoding.Default, "application/json"));
                if (!response.IsSuccessStatusCode)
                    throw new UnexpectedResponseException(response.StatusCode);
            }
        }
    }
}