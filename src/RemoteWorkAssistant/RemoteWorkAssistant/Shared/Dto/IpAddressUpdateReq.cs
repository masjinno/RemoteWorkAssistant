using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
