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
    public class UserController : ControllerBase
    {
        private readonly RemoteWorkAssistantContext _context;
        private UserRecordConverter _userRecordConverter;

        public UserController(RemoteWorkAssistantContext context)
        {
            this._context = context;
            this._userRecordConverter = new UserRecordConverter();
        }

        // POST: api/User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterReq userRegisterReq)
        {
            UserRecord userRecord = this._userRecordConverter.ConvertFromUserRegisterReq(userRegisterReq);

            if (this._context.ExistsUserRecord(userRecord.MailAddress))
            {
                return Conflict(new Error(Messages.EMAIL_CONFLICT));
            }

            this._context.UserTable.Add(userRecord);
            await this._context.SaveChangesAsync();

            return Ok();
        }




        // GET: api/v1/User
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserRecord>>> GetUserTable()
        {
            return await _context.UserTable.ToListAsync();
        }

        //// GET: api/v1/User/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserRecord>> GetUserRecord(string id)
        //{
        //    var userRecord = await _context.UserTable.FindAsync(id);

        //    if (userRecord == null)
        //    {
        //        return NotFound();
        //    }

        //    return userRecord;
        //}

        //// PUT: api/v1/User/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserRecord(string id, UserRecord userRecord)
        //{
        //    if (id != userRecord.MailAddress)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userRecord).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserRecordExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/v1/User/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserRecord(string id)
        //{
        //    var userRecord = await _context.UserTable.FindAsync(id);
        //    if (userRecord == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserTable.Remove(userRecord);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserRecordExists(string id)
        //{
        //    return _context.UserTable.Any(e => e.MailAddress == id);
        //}
    }
}
