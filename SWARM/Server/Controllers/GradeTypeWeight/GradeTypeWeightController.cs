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
    public class GradeTypeWeightController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGradeTypeWeight/{pSchoolId, pSectionId, pGradeTypeCode}")]
        public async Task<IActionResult> GetGradeTypeWeight(int pSchoolId, int pSectionId, String pGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.SectionId == pSectionId
                    && x.GradeTypeCode == pGradeTypeCode
                ).FirstOrDefaultAsync();

            return Ok(itmGradeTypeWeight);
        }

        [HttpDelete]
        [Route("DeleteGradeTypeWeight/{pSchoolId, pSectionId, pGradeTypeCode}")]
        public async Task<IActionResult> DeleteGradeTypeWeight(int pSchoolId, int pSectionId, String pGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.SectionId == pSectionId
                    && x.GradeTypeCode == pGradeTypeCode
                ).FirstOrDefaultAsync();
            _context.Remove(itmGradeTypeWeight);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdTpWt = await _context.GradeTypeWeights
                    .Where(
                        x => x.SchoolId == _GradeTypeWeight.SchoolId
                        && x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode
                    ).FirstOrDefaultAsync();
                if (_GrdTpWt == null)
                {
                    bExist = false;
                    _GrdTpWt = new GradeTypeWeight();
                }
                else
                    bExist = true;

                _GrdTpWt.SchoolId = _GradeTypeWeight.SchoolId;
                _GrdTpWt.SectionId = _GradeTypeWeight.SectionId;
                _GrdTpWt.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                _GrdTpWt.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                _GrdTpWt.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                _GrdTpWt.DropLowest = _GradeTypeWeight.DropLowest;
                _GrdTpWt.CreatedBy = _GradeTypeWeight.CreatedBy;
                _GrdTpWt.CreatedDate = _GradeTypeWeight.CreatedDate;
                _GrdTpWt.ModifiedBy = _GradeTypeWeight.ModifiedBy;
                _GrdTpWt.ModifiedDate = _GradeTypeWeight.ModifiedDate;
                if (bExist)
                    _context.GradeTypeWeights.Update(_GrdTpWt);
                else
                    _context.GradeTypeWeights.Add(_GrdTpWt);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdTpWt.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdTpWt = await _context.GradeTypeWeights
                    .Where(
                        x => x.SchoolId == _GradeTypeWeight.SchoolId
                        && x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode
                    ).FirstOrDefaultAsync();
                if (_GrdTpWt != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _GrdTpWt = new GradeTypeWeight
                {
                    SchoolId = _GradeTypeWeight.SchoolId,
                    SectionId = _GradeTypeWeight.SectionId,
                    GradeTypeCode = _GradeTypeWeight.GradeTypeCode,
                    NumberPerSection = _GradeTypeWeight.NumberPerSection,
                    PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade,
                    DropLowest = _GradeTypeWeight.DropLowest,
                    CreatedBy = _GradeTypeWeight.CreatedBy,
                    CreatedDate = _GradeTypeWeight.CreatedDate,
                    ModifiedBy = _GradeTypeWeight.ModifiedBy,
                    ModifiedDate = _GradeTypeWeight.ModifiedDate
                };
                _context.GradeTypeWeights.Add(_GrdTpWt);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdTpWt.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
