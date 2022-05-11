using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.GrdCv
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeConversionController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGradeConversion/{pSchoolId, pLetterGrade}")]
        public async Task<IActionResult> GetGradeConversion(int pSchoolId, String pLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.LetterGrade == pLetterGrade
                ).FirstOrDefaultAsync();

            return Ok(itmGradeConversion);
        }

        [HttpDelete]
        [Route("DeleteGradeConversion/{pSchoolId, pLetterGrade}")]
        public async Task<IActionResult> DeleteGradeConversion(int pSchoolId, String pLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.LetterGrade == pLetterGrade
                ).FirstOrDefaultAsync();
            _context.Remove(itmGradeConversion);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _GradeConversion)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdCv = await _context.GradeConversions
                    .Where(
                        x => x.SchoolId == _GradeConversion.SchoolId
                        && x.LetterGrade == _GradeConversion.LetterGrade
                    ).FirstOrDefaultAsync();
                if (_GrdCv == null)
                {
                    bExist = false;
                    _GrdCv = new GradeConversion();
                }
                else
                    bExist = true;

                _GrdCv.SchoolId = _GradeConversion.SchoolId;
                _GrdCv.LetterGrade = _GradeConversion.LetterGrade;
                _GrdCv.GradePoint = _GradeConversion.GradePoint;
                _GrdCv.MaxGrade = _GradeConversion.MaxGrade;
                _GrdCv.MinGrade = _GradeConversion.MinGrade;
                _GrdCv.CreatedBy = _GradeConversion.CreatedBy;
                _GrdCv.CreatedDate = _GradeConversion.CreatedDate;
                _GrdCv.ModifiedBy = _GradeConversion.ModifiedBy;
                _GrdCv.ModifiedDate = _GradeConversion.ModifiedDate;
                if (bExist)
                    _context.GradeConversions.Update(_GrdCv);
                else
                    _context.GradeConversions.Add(_GrdCv);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdCv.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _GradeConversion)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdCv = await _context.GradeConversions
                    .Where(
                        x => x.SchoolId == _GradeConversion.SchoolId
                        && x.LetterGrade == _GradeConversion.LetterGrade
                    ).FirstOrDefaultAsync();
                if (_GrdCv != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _GrdCv = new GradeConversion
                {
                    SchoolId = _GradeConversion.SchoolId,
                    LetterGrade = _GradeConversion.LetterGrade,
                    GradePoint = _GradeConversion.GradePoint,
                    MaxGrade = _GradeConversion.MaxGrade,
                    MinGrade = _GradeConversion.MinGrade,
                    CreatedBy = _GradeConversion.CreatedBy,
                    CreatedDate = _GradeConversion.CreatedDate,
                    ModifiedBy = _GradeConversion.ModifiedBy,
                    ModifiedDate = _GradeConversion.ModifiedDate
                };
                _context.GradeConversions.Add(_GrdCv);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdCv.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
