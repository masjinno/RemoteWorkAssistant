using System.Text.Json.Serialization;

namespace RemoteWorkAssistant.Shared.Dto
{
    public class IpAddressUpdateReq : UserAuthorization
    {
        [JsonPropertyName("pcName")]
        public string PcName { get; set; }

        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }
    }
}
