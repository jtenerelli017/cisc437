using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Crse
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public CourseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetCourse/{pCourseNo, pSchoolId}")]
        public async Task<IActionResult> GetCourse(int pCourseNo, int pSchoolId)
        {
            Course itmCourse = await _context.Courses
                .Where(
                    x => x.CourseNo == pCourseNo
                    && x.SchoolId == pSchoolId
                ).FirstOrDefaultAsync();

            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("DeleteCourse/{pCourseNo, pSchoolId}")]
        public async Task<IActionResult> DeleteCourse(int pCourseNo, int pSchoolId)
        {
            var enrollment = await _context.Enrollments
                .Include("section")
                .Include("course")
                .Where(
                    x => x.S.CourseNo == pCourseNo
                    && x.S.SchoolId == pSchoolId
                ).ToListAsync();
            foreach (var enr in enrollment)
            {
                _context.Enrollments.Remove(enr);
            }

            var section = await _context.Sections
                .Include("course")
                .Where(
                    x => x.CourseNo == pCourseNo
                    && x.SchoolId == pSchoolId
                ).ToListAsync();
            foreach (var sec in section)
            {
                _context.Sections.Remove(sec);
            }

            Course itmCourse = await _context.Courses
                .Where(
                    x => x.CourseNo == pCourseNo
                    && x.SchoolId == pSchoolId
                ).FirstOrDefaultAsync();
            _context.Remove(itmCourse);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Course)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Crse = await _context.Courses
                    .Where(
                        x => x.CourseNo == _Course.CourseNo
                        && x.SchoolId == _Course.SchoolId
                    ).FirstOrDefaultAsync();
                if (_Crse == null)
                {
                    bExist = false;
                    _Crse = new Course();
                }
                else
                    bExist = true;

                _Crse.CourseNo = _Course.CourseNo;
                _Crse.Description = _Course.Description;
                _Crse.Cost = _Course.Cost;
                _Crse.Prerequisite = _Course.Prerequisite;
                _Crse.CreatedBy = _Course.CreatedBy;
                _Crse.CreatedDate = _Course.CreatedDate;
                _Crse.ModifiedBy = _Course.ModifiedBy;
                _Crse.ModifiedDate = _Course.ModifiedDate;
                _Crse.SchoolId = _Course.SchoolId;
                _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                if (bExist)
                    _context.Courses.Update(_Crse);
                else
                    _context.Courses.Add(_Crse);
                _context.Update(_Crse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Crse.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Course)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Crse = await _context.Courses
                    .Where(
                        x => x.CourseNo == _Course.CourseNo
                        && x.SchoolId == _Course.SchoolId
                    ).FirstOrDefaultAsync();
                if (_Crse != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Crse = new Course
                {
                    CourseNo = _Course.CourseNo,
                    Description = _Course.Description,
                    Cost = _Course.Cost,
                    Prerequisite = _Course.Prerequisite,
                    CreatedBy = _Course.CreatedBy,
                    CreatedDate = _Course.CreatedDate,
                    ModifiedBy = _Course.ModifiedBy,
                    ModifiedDate = _Course.ModifiedDate,
                    SchoolId = _Course.SchoolId,
                    PrerequisiteSchoolId = _Course.PrerequisiteSchoolId
                };
                _context.Courses.Add(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Crse.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
