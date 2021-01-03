using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RemoteWorkAssistant.Server.Constants;
using RemoteWorkAssistant.Server.Converters;
using RemoteWorkAssistant.Server.Models;
using RemoteWorkAssistant.Shared.Dto;

namespace RemoteWorkAssistant.Server.Controllers
{
    /// <summary>
    /// 参考：https://docs.microsoft.com/ja-jp/aspnet/web-api/overview/security/individual-accounts-in-web-api
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly RemoteWorkAssistantContext _context;
        private IConfiguration _config;

        public UserController(RemoteWorkAssistantContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterReq userRegisterReq)
        {
            UserRecord userRecord = UserRecordConverter.ConvertFromUserPostReq(userRegisterReq);

            if (this._context.ExistsUserRecord(userRecord.MailAddress))
            {
                return Conflict(new Error(Messages.EMAIL_CONFLICT));
            }

            this._context.UserTable.Add(userRecord);
            await this._context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/User/5
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody]UserLoginReq userLoginReq)
        {
            if (string.IsNullOrEmpty(userLoginReq.MailAddress) || string.IsNullOrEmpty(userLoginReq.Password))
            {
                return Unauthorized(new Error(Messages.AUTHENTICATION_ERROR));
            }

            if (!this._context.ExistsUserRecord(userLoginReq.MailAddress)) {
                return Unauthorized(new Error(Messages.AUTHENTICATION_ERROR));
            }
            UserRecord userRecord = await this._context.UserTable.FindAsync(userLoginReq.MailAddress);
            if (userRecord.Password.Equals(userLoginReq.Password))
            {
                return Ok(new UserLoginResp { AccessToken = this.BuildToken(userRecord) });
            }
            else
            {
                return Unauthorized(new Error(Messages.AUTHENTICATION_ERROR));
            }
        }

        //Token生成
        private string BuildToken(UserRecord user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddHours(24), // トークンの有効期限は1日
              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }







        // GET: api/v1/User
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserRecord>>> GetUserTable()
        {
            return await _context.UserTable.ToListAsync();
        }

        // GET: api/v1/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRecord>> GetUserRecord(string id)
        {
            var userRecord = await _context.UserTable.FindAsync(id);

            if (userRecord == null)
            {
                return NotFound();
            }

            return userRecord;
        }

        // PUT: api/v1/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRecord(string id, UserRecord userRecord)
        {
            if (id != userRecord.MailAddress)
            {
                return BadRequest();
            }

            _context.Entry(userRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/v1/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRecord(string id)
        {
            var userRecord = await _context.UserTable.FindAsync(id);
            if (userRecord == null)
            {
                return NotFound();
            }

            _context.UserTable.Remove(userRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRecordExists(string id)
        {
            return _context.UserTable.Any(e => e.MailAddress == id);
        }
    }
}
