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
    public class GradeController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetGrade/{pSchoolId, pStudentId, pSectionId, pGradeTypeCode, pGradeCodeOccurrence}")]
        public async Task<IActionResult> GetGrade(int pSchoolId, int pStudentId, int pSectionId, String pGradeTypeCode, int pGradeCodeOccurrence)
        {
            Grade itmGrade = await _context.Grades
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.StudentId == pStudentId
                    && x.SectionId == pSectionId
                    && x.GradeTypeCode == pGradeTypeCode
                    && x.GradeCodeOccurrence == pGradeCodeOccurrence
                ).FirstOrDefaultAsync();

            return Ok(itmGrade);
        }

        [HttpDelete]
        [Route("DeleteGrade/{pSchoolId, pStudentId, pSectionId, pGradeTypeCode, pGradeCodeOccurrence}")]
        public async Task<IActionResult> DeleteGrade(int pSchoolId, int pStudentId, int pSectionId, String pGradeTypeCode, int pGradeCodeOccurrence)
        {
            Grade itmGrade = await _context.Grades
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.StudentId == pStudentId
                    && x.SectionId == pSectionId
                    && x.GradeTypeCode == pGradeTypeCode
                    && x.GradeCodeOccurrence == pGradeCodeOccurrence
                ).FirstOrDefaultAsync();
            _context.Remove(itmGrade);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Grd = await _context.Grades
                    .Where(
                        x => x.SchoolId == _Grade.SchoolId
                        && x.StudentId == _Grade.StudentId
                        && x.SectionId == _Grade.SectionId
                        && x.GradeTypeCode == _Grade.GradeTypeCode
                        && x.GradeCodeOccurrence == _Grade.GradeCodeOccurrence
                    ).FirstOrDefaultAsync();
                if (_Grd == null)
                {
                    bExist = false;
                    _Grd = new Grade();
                }
                else
                    bExist = true;

                _Grd.SchoolId = _Grade.SchoolId;
                _Grd.StudentId = _Grade.StudentId;
                _Grd.SectionId = _Grade.SectionId;
                _Grd.GradeTypeCode = _Grade.GradeTypeCode;
                _Grd.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                _Grd.NumericGrade = _Grade.NumericGrade;
                _Grd.Comments = _Grade.Comments;
                _Grd.CreatedBy = _Grade.CreatedBy;
                _Grd.CreatedDate = _Grade.CreatedDate;
                _Grd.ModifiedBy = _Grade.ModifiedBy;
                _Grd.ModifiedDate = _Grade.ModifiedDate;
                if (bExist)
                    _context.Grades.Update(_Grd);
                else
                    _context.Grades.Add(_Grd);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grd.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Grade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Grd = await _context.Grades
                    .Where(
                        x => x.SchoolId == _Grade.SchoolId
                        && x.StudentId == _Grade.StudentId
                        && x.SectionId == _Grade.SectionId
                        && x.GradeTypeCode == _Grade.GradeTypeCode
                        && x.GradeCodeOccurrence == _Grade.GradeCodeOccurrence
                    ).FirstOrDefaultAsync();
                if (_Grd != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Grd = new Grade
                {
                    SchoolId = _Grade.SchoolId,
                    StudentId = _Grade.StudentId,
                    SectionId = _Grade.SectionId,
                    GradeTypeCode = _Grade.GradeTypeCode,
                    GradeCodeOccurrence = _Grade.GradeCodeOccurrence,
                    NumericGrade = _Grade.NumericGrade,
                    Comments = _Grade.Comments,
                    CreatedBy = _Grade.CreatedBy,
                    CreatedDate = _Grade.CreatedDate,
                    ModifiedBy = _Grade.ModifiedBy,
                    ModifiedDate = _Grade.ModifiedDate
                };
                _context.Grades.Add(_Grd);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grd.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
