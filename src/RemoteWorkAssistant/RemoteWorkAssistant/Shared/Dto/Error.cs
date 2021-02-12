using System.Text.Json.Serialization;

namespace RemoteWorkAssistant.Shared.Dto
{
    public class Error
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
