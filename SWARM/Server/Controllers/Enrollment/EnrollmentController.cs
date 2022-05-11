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
    public class EnrollmentController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetEnrollment/{pStudentId, pSectionId, pSchoolId}")]
        public async Task<IActionResult> GetEnrollment(int pStudentId, int pSectionId, int pSchoolId)
        {
            Enrollment itmEnrollment = await _context.Enrollments
                .Where(
                    x => x.StudentId == pStudentId
                    && x.SectionId == pSectionId
                    && x.SchoolId == pSchoolId
                ).FirstOrDefaultAsync();

            return Ok(itmEnrollment);
        }

        [HttpDelete]
        [Route("DeleteEnrollment/{pStudentId, pSectionId, pSchoolId}")]
        public async Task<IActionResult> DeleteEnrollment(int pStudentId, int pSectionId, int pSchoolId)
        {
            Enrollment itmEnrollment = await _context.Enrollments
                .Where(
                    x => x.StudentId == pStudentId
                    && x.SectionId == pSectionId
                    && x.SchoolId == pSchoolId
                ).FirstOrDefaultAsync();
            _context.Remove(itmEnrollment);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Enr = await _context.Enrollments
                    .Where(
                        x => x.StudentId == _Enrollment.StudentId
                        && x.SectionId == _Enrollment.SectionId
                        && x.SchoolId == _Enrollment.SchoolId
                    ).FirstOrDefaultAsync();
                if (_Enr == null)
                {
                    bExist = false;
                    _Enr = new Enrollment();
                }
                else
                    bExist = true;

                _Enr.StudentId = _Enrollment.StudentId;
                _Enr.SectionId = _Enrollment.SectionId;
                _Enr.EnrollDate = _Enrollment.EnrollDate;
                _Enr.FinalGrade = _Enrollment.FinalGrade;
                _Enr.CreatedBy = _Enrollment.CreatedBy;
                _Enr.CreatedDate = _Enrollment.CreatedDate;
                _Enr.ModifiedBy = _Enrollment.ModifiedBy;
                _Enr.ModifiedDate = _Enrollment.ModifiedDate;
                _Enr.SchoolId = _Enrollment.SchoolId;
                if (bExist)
                    _context.Enrollments.Update(_Enr);
                else
                    _context.Enrollments.Add(_Enr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enr.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Enr = await _context.Enrollments
                    .Where(
                        x => x.StudentId == _Enrollment.StudentId
                        && x.SectionId == _Enrollment.SectionId
                        && x.SchoolId == _Enrollment.SchoolId
                    ).FirstOrDefaultAsync();
                if (_Enr != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Enr = new Enrollment
                {
                    StudentId = _Enrollment.StudentId,
                    SectionId = _Enrollment.SectionId,
                    EnrollDate = _Enrollment.EnrollDate,
                    FinalGrade = _Enrollment.FinalGrade,
                    CreatedBy = _Enrollment.CreatedBy,
                    CreatedDate = _Enrollment.CreatedDate,
                    ModifiedBy = _Enrollment.ModifiedBy,
                    ModifiedDate = _Enrollment.ModifiedDate,
                    SchoolId = _Enrollment.SchoolId
                };
                _context.Enrollments.Add(_Enr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enr.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
