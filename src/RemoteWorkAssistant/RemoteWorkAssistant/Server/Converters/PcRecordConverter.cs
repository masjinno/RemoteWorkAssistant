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
        public PcRecord ConvertFromPcRegisterReq(PcRegisterReq pcRegisterReq)
        {
            return new PcRecord
            {
                Id = RemoteWorkAssistantContext.GeneratePcInfoId(
                    pcRegisterReq.MailAddress, pcRegisterReq.PcName),
                MailAddress = pcRegisterReq.MailAddress,
                PcName = pcRegisterReq.PcName,
                IpAddress = null,
                UpdatedDateTime = null
            };
        }

        internal PcRecord ConvertFromIpAddressUpdateReq(IpAddressUpdateReq ipAddressUpdateReq, string updatedDateTime)
        {
            return new PcRecord
            {
                Id = RemoteWorkAssistantContext.GeneratePcInfoId(
                    ipAddressUpdateReq.MailAddress, ipAddressUpdateReq.PcName),
                MailAddress = ipAddressUpdateReq.MailAddress,
                PcName = ipAddressUpdateReq.PcName,
                IpAddress = ipAddressUpdateReq.IpAddress,
                UpdatedDateTime = updatedDateTime
            };
        }
    }
}
