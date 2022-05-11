using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Enr
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeTypeController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGradeType/{pSchoolId, pGradeTypeCode}")]
        public async Task<IActionResult> GetGradeType(int pSchoolId, String pGradeTypeCode)
        {
            GradeType itmGradeType = await _context.GradeTypes
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.GradeTypeCode == pGradeTypeCode
                ).FirstOrDefaultAsync();

            return Ok(itmGradeType);
        }

        [HttpDelete]
        [Route("DeleteGradeType/{pSchoolId, pGradeTypeCode}")]
        public async Task<IActionResult> DeleteGradeType(int pSchoolId, String pGradeTypeCode)
        {
            GradeType itmGradeType = await _context.GradeTypes
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.GradeTypeCode == pGradeTypeCode
                ).FirstOrDefaultAsync();
            _context.Remove(itmGradeType);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeType)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdTp = await _context.GradeTypes
                    .Where(
                        x => x.SchoolId == _GradeType.SchoolId
                        && x.GradeTypeCode == _GradeType.GradeTypeCode
                    ).FirstOrDefaultAsync();
                if (_GrdTp == null)
                {
                    bExist = false;
                    _GrdTp = new GradeType();
                }
                else
                    bExist = true;

                _GrdTp.SchoolId = _GradeType.SchoolId;
                _GrdTp.GradeTypeCode = _GradeType.GradeTypeCode;
                _GrdTp.Description = _GradeType.Description;
                _GrdTp.CreatedBy = _GradeType.CreatedBy;
                _GrdTp.CreatedDate = _GradeType.CreatedDate;
                _GrdTp.ModifiedBy = _GradeType.ModifiedBy;
                _GrdTp.ModifiedDate = _GradeType.ModifiedDate;
                if (bExist)
                    _context.GradeTypes.Update(_GrdTp);
                else
                    _context.GradeTypes.Add(_GrdTp);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdTp.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _GradeType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdTp = await _context.GradeTypes
                    .Where(
                        x => x.SchoolId == _GradeType.SchoolId
                        && x.GradeTypeCode == _GradeType.GradeTypeCode
                    ).FirstOrDefaultAsync();
                if (_GrdTp != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _GrdTp = new GradeType
                {
                    SchoolId = _GradeType.SchoolId,
                    GradeTypeCode = _GradeType.GradeTypeCode,
                    Description = _GradeType.Description,
                    CreatedBy = _GradeType.CreatedBy,
                    CreatedDate = _GradeType.CreatedDate,
                    ModifiedBy = _GradeType.ModifiedBy,
                    ModifiedDate = _GradeType.ModifiedDate
                };
                _context.GradeTypes.Add(_GrdTp);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdTp.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
