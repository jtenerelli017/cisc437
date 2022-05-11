using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Schl
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetSchool/{pSchoolId}")]
        public async Task<IActionResult> GetSchool(int pSchoolId)
        {
            School itmSchool = await _context.Schools
                .Where(
                    x => x.SchoolId == pSchoolId
                ).FirstOrDefaultAsync();

            return Ok(itmSchool);
        }

        [HttpDelete]
        [Route("DeleteSchool/{pSchoolId}")]
        public async Task<IActionResult> DeleteSchool(int pSchoolId)
        {
            School itmSchool = await _context.Schools
               .Where(
                   x => x.SchoolId == pSchoolId
               ).FirstOrDefaultAsync();
            _context.Remove(itmSchool);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _School)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Schl = await _context.Schools
                    .Where(
                        x => x.SchoolId == _School.SchoolId
                    ).FirstOrDefaultAsync();
                if (_Schl == null)
                {
                    bExist = false;
                    _Schl = new School();
                }
                else
                    bExist = true;

                _Schl.SchoolId = _School.SchoolId;
                _Schl.SchoolName = _School.SchoolName;
                _Schl.CreatedBy = _School.CreatedBy;
                _Schl.CreatedDate = _School.CreatedDate;
                _Schl.ModifiedBy = _School.ModifiedBy;
                _Schl.ModifiedDate = _School.ModifiedDate;
                if (bExist)
                    _context.Schools.Update(_Schl);
                else
                    _context.Schools.Add(_Schl);
                
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Schl.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School _School)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Schl = await _context.Schools
                    .Where(
                        x => x.SchoolId == _School.SchoolId
                    ).FirstOrDefaultAsync();
                if (_Schl != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Schl = new School
                {
                    SchoolId = _School.SchoolId,
                    SchoolName = _School.SchoolName,
                    CreatedBy = _School.CreatedBy,
                    CreatedDate = _School.CreatedDate,
                    ModifiedBy = _School.ModifiedBy,
                    ModifiedDate = _School.ModifiedDate
                };
                _context.Schools.Add(_Schl);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Schl.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
