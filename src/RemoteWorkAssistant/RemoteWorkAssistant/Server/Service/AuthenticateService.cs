using RemoteWorkAssistant.Server.Models;
using RemoteWorkAssistant.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Server.Service
{
    public class AuthenticateService
    {
        private readonly RemoteWorkAssistantContext _context;

        public AuthenticateService(RemoteWorkAssistantContext context)
        {
            this._context = context;
        }

        public bool Authenticate(UserAuthorization userData)
        {
            return this._context.UserTable.Any(ui => ui.MailAddress.Equals(userData.MailAddress) && ui.Password.Equals(userData.Password));
        }
    }
}
