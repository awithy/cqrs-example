using Newtonsoft.Json;

namespace Api.DTOs
{
    public class CreateReservationRequest
    {
        [JsonProperty("memberId")]
        public string MemberId { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("sessionLocation")]
        public string SessionLocation { get; set; }
    }
}