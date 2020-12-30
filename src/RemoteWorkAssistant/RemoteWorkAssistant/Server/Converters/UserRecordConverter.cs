using RemoteWorkAssistant.Server.Models;
using RemoteWorkAssistant.Shared.Dto;

namespace RemoteWorkAssistant.Server.Converters
{
    public class UserRecordConverter
    {
        public static UserRecord ConvertFromUserPostReq(UserRegisterReq userRegisterReq)
        {
            return new UserRecord
            {
                MailAddress = userRegisterReq.MailAddress,
                Password = userRegisterReq.Password
            };
        }
    }
}
