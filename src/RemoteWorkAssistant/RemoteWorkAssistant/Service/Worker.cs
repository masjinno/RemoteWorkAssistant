using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RemoteWorkAssistant.Service.Model;
using RemoteWorkAssistant.Shared.Dto;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static readonly int INTERVAL_MINUTES = 3;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                this.putIpAddress("kamoshi_moyashi@yahoo.co.jp", "1234", Environment.MachineName);

                await Task.Delay(INTERVAL_MINUTES * 60 * 1000, stoppingToken);
            }
        }

        private async void putIpAddress(string mailAddress, string password, string pcName)
        {
            RWAServerCaller caller = new RWAServerCaller("https://remoteworkassistantserver20201224.azurewebsites.net/", this._logger);
            string ipAddress = IpAddressUtil.getIpAddress();

            IpAddressUpdateReq ipAddressUpdateReq = new IpAddressUpdateReq
            {
                MailAddress = mailAddress,
                Password = password,
                PcName = pcName,
                IpAddress = ipAddress
            };

            HttpResponseMessage ipAddressPutResp = await caller.UpdateIpAddress(ipAddressUpdateReq);

            if (ipAddressPutResp.StatusCode == HttpStatusCode.BadRequest)
            {
                // NotFoundÇæÇ¡ÇΩèÍçáÅAÇ‡Ç§àÍìxìoò^Ç©ÇÁçsÇ§
                UserRegisterReq userRegisterReq = new UserRegisterReq
                {
                    MailAddress = mailAddress,
                    Password = password
                };
                await caller.RegisterUser(userRegisterReq);
                PcRegisterReq pcRegisterReq = new PcRegisterReq
                {
                    MailAddress = mailAddress,
                    Password = password,
                    PcName = pcName
                };
                await caller.RegisterPc(pcRegisterReq);
                await caller.UpdateIpAddress(ipAddressUpdateReq);
            }
        }
    }
}
