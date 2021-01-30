using System.Text.Json.Serialization;

namespace RemoteWorkAssistant.Shared.Dto
{
    public abstract class UserAuthorization
    {
        [JsonPropertyName("email")]
        public string MailAddress { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
