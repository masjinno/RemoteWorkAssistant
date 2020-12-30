using RemoteWorkAssistant.Server.Constants;

namespace RemoteWorkAssistant.Server.Models
{
    public class Error
    {
        public Error(Messages message)
        {
            this.Message = message.GetStringValue();
        }

        public string Message { get; set; }
    }
}
