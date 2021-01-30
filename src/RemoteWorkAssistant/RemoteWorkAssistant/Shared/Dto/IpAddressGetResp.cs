using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Shared.Dto
{
    public class IpAddressGetResp
    {
        [JsonPropertyName("email")]
        public string MailAddress { get; set; }

        [JsonPropertyName("pc")]
        public List<PcInfo> PcData { get; set; }
    }
}
