using RemoteWorkAssistant.Server.Models;
using RemoteWorkAssistant.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Server.Converters
{
    public class PcRecordConverter
    {
        private readonly RemoteWorkAssistantContext _context;

        public PcRecordConverter(RemoteWorkAssistantContext context)
        {
            this._context = context;
        }

        public PcRecord ConvertFromPcRegisterReq(PcRegisterReq pcRegisterReq)
        {
            return new PcRecord
            {
                Id = RemoteWorkAssistantContext.GeneratePcInfoId(
                    pcRegisterReq.MailAddress, pcRegisterReq.PcName),
                MailAddress = pcRegisterReq.MailAddress,
                PcName = pcRegisterReq.PcName,
                IpAddress = null
            };
        }

        internal PcRecord ConvertFromIpAddressUpdateReq(IpAddressUpdateReq ipAddressUpdateReq)
        {
            return new PcRecord
            {
                Id = RemoteWorkAssistantContext.GeneratePcInfoId(
                    ipAddressUpdateReq.MailAddress, ipAddressUpdateReq.PcName),
                MailAddress = ipAddressUpdateReq.MailAddress,
                PcName = ipAddressUpdateReq.PcName,
                IpAddress = ipAddressUpdateReq.IpAddress
            };
        }
    }
}
