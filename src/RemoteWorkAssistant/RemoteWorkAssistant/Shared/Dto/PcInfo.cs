using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Shared.Dto
{
    public class PcInfo
    {
        [JsonPropertyName("pcName")]
        public string PcName { get; set; }

        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }

        [JsonPropertyName("updatedDateTime")]
        public string UpdatedDateTime { get; set; }
    }
}
