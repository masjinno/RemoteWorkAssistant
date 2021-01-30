using System.Text.Json.Serialization;

namespace RemoteWorkAssistant.Shared.Dto
{
    public class PcRegisterReq : UserAuthorization
    {
        [JsonPropertyName("pcName")]
        public string PcName { get; set; }
    }
}
