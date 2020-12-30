using System.Text.Json.Serialization;

namespace RemoteWorkAssistant.Shared.Dto
{
    public class UserRegisterReq
    {
        [JsonPropertyName("email")]
        public string MailAddress { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
