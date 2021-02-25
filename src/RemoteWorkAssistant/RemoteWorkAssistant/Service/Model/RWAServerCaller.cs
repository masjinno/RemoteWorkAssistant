using Microsoft.Extensions.Logging;
using RemoteWorkAssistant.Shared.Dto;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Service.Model
{
    class RWAServerCaller : ApiCaller
    {
        private readonly ILogger<object> _logger;

        public RWAServerCaller(string baseUri, ILogger<object> logger) : base(baseUri)
        {
            this._logger = logger;
        }

        public async Task<HttpResponseMessage> RegisterUser(UserRegisterReq reqBody)
        {
            string path = "/api/v1/user";
            HttpResponseMessage resp = await this.Post(path, reqBody);
            _logger.LogInformation("Post {0} -> response: status={0}", path, resp.StatusCode);
            return resp;
        }

        public async Task<HttpResponseMessage> RegisterPc(PcRegisterReq reqBody)
        {
            string path = "/api/v1/pc";
            HttpResponseMessage resp = await this.Post(path, reqBody);
            _logger.LogInformation("Post {0} -> response: status={0}", path, resp.StatusCode);
            return resp;
        }

        public async Task<HttpResponseMessage> UpdateIpAddress(IpAddressUpdateReq reqBody)
        {
            string path = "/api/v1/pc/ipaddress";
            HttpResponseMessage resp = await this.Put(path, reqBody);
            _logger.LogInformation("Put {0} -> response: status={0}", path, resp.StatusCode);
            return resp;
        }
    }
}
