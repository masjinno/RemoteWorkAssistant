using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteWorkAssistant.Server.Constants;
using RemoteWorkAssistant.Server.Converters;
using RemoteWorkAssistant.Server.Models;
using RemoteWorkAssistant.Server.Service;
using RemoteWorkAssistant.Shared.Dto;

namespace RemoteWorkAssistant.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PcController : ControllerBase
    {
        private readonly RemoteWorkAssistantContext _context;
        private AuthenticateService _authService;
        private PcRecordConverter _pcRecordConverter;

        public PcController(RemoteWorkAssistantContext context)
        {
            this._context = context;
            this._authService = new AuthenticateService(context);
            this._pcRecordConverter = new PcRecordConverter();
        }

        // POST: api/v1/pc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> RegisterPc(PcRegisterReq pcRegisterReq)
        {
            if (!this._authService.Authenticate(pcRegisterReq))
            {
                return BadRequest(new Error(Messages.AUTHENTICATION_ERROR));
            }

            PcRecord pcInfo = this._pcRecordConverter.ConvertFromPcRegisterReq(pcRegisterReq);
            if (this._context.ExistsPcRecord(pcInfo.Id))
            {
                return Conflict(new Error(Messages.PC_NAME_CONFLICT));
            }

            this._context.PcTable.Add(pcInfo);
            await this._context.SaveChangesAsync();

            return Ok();
        }

        //// PUT: api/v1/pc
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut]
        //public async Task<IActionResult> PutPc(PcPutReq pcPutReq)
        //{
        //    if (!this._authService.Authenticate(pcRegisterReq))
        //    {
        //        return BadRequest(new Error(Messages.AUTHENTICATION_ERROR));
        //    }

        //    PcRecord pcInfo = pcPutReq.ConvertToPcRecord();
        //    if (this._context.PcRecordExists(RemoteWorkAssistantContext.GeneratePcInfoId(pcInfo.MailAddress, pcInfo.PcName)))
        //    {
        //        return Conflict(new Error(Messages.PC_NAME_CONFLICT));
        //    }

        //    this._context.Entry(pcInfo).State = EntityState.Modified;

        //    try
        //    {
        //        await this._context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!this._context.PcRecordExists(pcInfo.Id))
        //        {
        //            return NotFound(new Error(Messages.PC_NAME_NOT_FOUND));
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok();
        //}

        // PUT: api/v1/pc/ipaddress
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ipaddress")]
        public async Task<IActionResult> PutIpAddress(IpAddressUpdateReq ipAddressUpdateReq)
        {
            if (!this._authService.Authenticate(ipAddressUpdateReq))
            {
                return BadRequest(new Error(Messages.AUTHENTICATION_ERROR));
            }

            DateTime requestedDateTimeUtc = DateTime.UtcNow;
            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime tstDateTime = TimeZoneInfo.ConvertTimeFromUtc(requestedDateTimeUtc, tst);
            string iso8601DateTime = tstDateTime.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
            PcRecord pcInfo = this._pcRecordConverter.ConvertFromIpAddressUpdateReq(ipAddressUpdateReq, iso8601DateTime);
            this._context.Entry(pcInfo).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this._context.ExistsPcRecord(pcInfo.Id))
                {
                    return NotFound(new Error(Messages.PC_NAME_NOT_FOUND));
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // PUT: api/v1/pc/ipaddress
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ipaddress/get")]
        public async Task<ActionResult<IpAddressGetResp>> PutIpAddressGet(UserAuthorization ipAddressGetReq)
        {
            if (!this._authService.Authenticate(ipAddressGetReq))
            {
                return BadRequest(new Error(Messages.AUTHENTICATION_ERROR));
            }

            IpAddressGetResp resp = new IpAddressGetResp
            {
                MailAddress = ipAddressGetReq.MailAddress,
                PcData = null
            };
            resp.PcData = await this._context.PcTable
                .Where(pr => pr.MailAddress.Equals(ipAddressGetReq.MailAddress))
                .Select(pr => new PcInfo
                {
                    PcName = pr.PcName,
                    IpAddress = pr.IpAddress,
                    UpdatedDateTime = pr.UpdatedDateTime
                })
                .ToListAsync();

            return Ok(resp);
        }

        // For Debug
        // GET: api/v1/pc/debug
        [HttpGet("debug")]
        public async Task<ActionResult<IEnumerable<PcRecord>>> GetPcInfos()
        {
            return await _context.PcTable.ToListAsync();
        }




        //// GET: api/Pc
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PcRecord>>> GetPcTable()
        //{
        //    return await _context.PcTable.ToListAsync();
        //}

        //// GET: api/Pc/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PcRecord>> GetPcRecord(string id)
        //{
        //    var pcRecord = await _context.PcTable.FindAsync(id);

        //    if (pcRecord == null)
        //    {
        //        return NotFound();
        //    }

        //    return pcRecord;
        //}

        //// PUT: api/Pc/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPcRecord(string id, PcRecord pcRecord)
        //{
        //    if (id != pcRecord.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(pcRecord).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PcRecordExists(id))
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

        //// DELETE: api/Pc/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePcRecord(string id)
        //{
        //    var pcRecord = await _context.PcTable.FindAsync(id);
        //    if (pcRecord == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.PcTable.Remove(pcRecord);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool PcRecordExists(string id)
        //{
        //    return _context.PcTable.Any(e => e.Id == id);
        //}
    }
}
