using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Zpcd
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipcodeController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetZipcode/{pZip}")]
        public async Task<IActionResult> GetZipcode(String pZip)
        {
            Zipcode itmZipcode = await _context.Zipcodes
                .Where(
                    x => x.Zip == pZip
                ).FirstOrDefaultAsync();

            return Ok(itmZipcode);
        }

        [HttpDelete]
        [Route("DeleteZipcode/{pZip}")]
        public async Task<IActionResult> DeleteZipcode(String pZip)
        {
            Zipcode itmZipcode = await _context.Zipcodes
                .Where(
                    x => x.Zip == pZip
                ).FirstOrDefaultAsync();
            _context.Remove(itmZipcode);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Zipcode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zpcd = await _context.Zipcodes
                    .Where(
                        x => x.Zip == _Zipcode.Zip
                    ).FirstOrDefaultAsync();
                if (_Zpcd == null)
                {
                    bExist = false;
                    _Zpcd = new Zipcode();
                }
                else
                    bExist = true;

                _Zpcd.Zip = _Zipcode.Zip;
                _Zpcd.City = _Zipcode.City;
                _Zpcd.State = _Zipcode.State;
                _Zpcd.CreatedBy = _Zipcode.CreatedBy;
                _Zpcd.CreatedDate = _Zipcode.CreatedDate;
                _Zpcd.ModifiedBy = _Zipcode.ModifiedBy;
                _Zpcd.ModifiedDate = _Zipcode.ModifiedDate;
                if (bExist)
                    _context.Zipcodes.Update(_Zpcd);
                else
                    _context.Zipcodes.Add(_Zpcd);
                
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zpcd.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Zipcode)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zpcd = await _context.Zipcodes
                    .Where(
                        x => x.Zip == _Zipcode.Zip
                    ).FirstOrDefaultAsync();
                if (_Zpcd != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Zpcd = new Zipcode
                {
                    Zip = _Zipcode.Zip,
                    City = _Zipcode.City,
                    State = _Zipcode.State,
                    CreatedBy = _Zipcode.CreatedBy,
                    CreatedDate = _Zipcode.CreatedDate,
                    ModifiedBy = _Zipcode.ModifiedBy,
                    ModifiedDate = _Zipcode.ModifiedDate
                };
                _context.Zipcodes.Add(_Zpcd);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zpcd.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
