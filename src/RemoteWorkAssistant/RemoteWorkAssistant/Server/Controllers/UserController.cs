using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteWorkAssistant.Server.Constants;
using RemoteWorkAssistant.Server.Converters;
using RemoteWorkAssistant.Server.Models;
using RemoteWorkAssistant.Shared.Dto;

namespace RemoteWorkAssistant.Server.Controllers
{
    /// <summary>
    /// 参考：https://docs.microsoft.com/ja-jp/aspnet/web-api/overview/security/individual-accounts-in-web-api
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly RemoteWorkAssistantContext _context;

        public UserController(RemoteWorkAssistantContext context)
        {
            _context = context;
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserRecord>> RegisterUser(UserRegisterReq userRegisterReq)
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginReq userLoginReq)
        {
            if (!this._context.ExistsUserRecord(userLoginReq.MailAddress)) {
                return Unauthorized(new Error(Messages.AUTHENTICATION_ERROR));
            }
            UserRecord userRecord = await this._context.UserTable.FindAsync(userLoginReq.MailAddress);
            if (userRecord.Password.Equals(userLoginReq.Password))
            {
                return Ok(new UserLoginResp());
            }
            else
            {
                return Unauthorized(new Error(Messages.AUTHENTICATION_ERROR));
            }
        }









        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRecord>>> GetUserTable()
        {
            return await _context.UserTable.ToListAsync();
        }

        // GET: api/User/5
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

        // PUT: api/User/5
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserRecord>> PostUserRecord(UserRecord userRecord)
        {
            _context.UserTable.Add(userRecord);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserRecordExists(userRecord.MailAddress))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserRecord", new { id = userRecord.MailAddress }, userRecord);
        }

        // DELETE: api/User/5
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
