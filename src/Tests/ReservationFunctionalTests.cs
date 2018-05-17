using Api;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Api.Clients;
using Api.Common;
using Api.DTOs;
using Xunit;

namespace Tests
{
    public class TestConfig : IApiConfig
    {
        public Uri BaseUri => new Uri("http://localhost:5000");
    }

    public class ReservationFunctionalTests
    {
        [Fact]
        public async Task CreateReservation()
        {
            using (var testApi = new TestApi())
            {
                await testApi.Start();

                var testConfig = new TestConfig();
                var reservationClient = new ReservationsClient(testConfig.BaseUri);
                var createReservation = new CreateReservationRequest
                {
                    SessionId = "sessionId123",
                    MemberId = "memberId123",
                };
                await reservationClient.Create(createReservation);

                await testApi.Stop();
            }
        }
    }

    public class TestApi : IDisposable
    {
        private Thread _thread;
        private bool _stopFlag = false;
        private bool _stopped = false;

        public async Task Start()
        {
            var threadStart = new ThreadStart(_Target);
            _thread = new Thread(threadStart);
            _thread.IsBackground = true;
            _thread.Start();
            while (true)
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.GetAsync(new Uri("http://localhost:5000"));
                    if (result.StatusCode == HttpStatusCode.NotFound)
                        break;
                }
            }
        }

        public Task Stop()
        {
            Program.Host.StopAsync().Wait();
            _thread.Join(TimeSpan.FromSeconds(15));
            _stopped = true;
            return Task.CompletedTask;
        }

        private void _Target()
        {
            Program.Main(null);
        }

        public void Dispose()
        {
            if (!_stopped)
            {
                Task.Run(async () => await Program.Host.StopAsync());
            }
        }
    }
}