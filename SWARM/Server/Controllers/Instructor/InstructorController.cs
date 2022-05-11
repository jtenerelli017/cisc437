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
    public class InstructorController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetInstructor/{pSchoolId, pInstructorId}")]
        public async Task<IActionResult> GetInstructor(int pSchoolId, int pInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.InstructorId == pInstructorId
                ).FirstOrDefaultAsync();

            return Ok(itmInstructor);
        }

        [HttpDelete]
        [Route("DeleteCourse/{pCourseNo, pSchoolId}")]
        public async Task<IActionResult> DeleteInstructor(int pSchoolId, int pInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors
                .Where(
                    x => x.SchoolId == pSchoolId
                    && x.InstructorId == pInstructorId
                ).FirstOrDefaultAsync();
            _context.Remove(itmInstructor);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Instr = await _context.Instructors
                    .Where(
                        x => x.SchoolId == _Instructor.SchoolId
                        && x.InstructorId == _Instructor.InstructorId
                    ).FirstOrDefaultAsync();
                if (_Instr == null)
                {
                    bExist = false;
                    _Instr = new Instructor();
                }
                else
                    bExist = true;

                _Instr.SchoolId = _Instructor.SchoolId;
                _Instr.InstructorId = _Instructor.InstructorId;
                _Instr.Salutation = _Instructor.Salutation;
                _Instr.FirstName = _Instructor.FirstName;
                _Instr.LastName = _Instructor.LastName;
                _Instr.StreetAddress = _Instructor.StreetAddress;
                _Instr.Zip = _Instructor.Zip;
                _Instr.Phone = _Instructor.Phone;
                _Instr.CreatedBy = _Instructor.CreatedBy;
                _Instr.CreatedDate = _Instructor.CreatedDate;
                _Instr.ModifiedBy = _Instructor.ModifiedBy;
                _Instr.ModifiedDate = _Instructor.ModifiedDate;
                if (bExist)
                    _context.Instructors.Update(_Instr);
                else
                    _context.Instructors.Add(_Instr);
                
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instr.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Instr = await _context.Instructors
                    .Where(
                        x => x.SchoolId == _Instructor.SchoolId
                        && x.InstructorId == _Instructor.InstructorId
                    ).FirstOrDefaultAsync();
                if (_Instr != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Instr = new Instructor
                {
                    SchoolId = _Instructor.SchoolId,
                    InstructorId = _Instructor.InstructorId,
                    Salutation = _Instructor.Salutation,
                    FirstName = _Instructor.FirstName,
                    LastName = _Instructor.LastName,
                    StreetAddress = _Instructor.StreetAddress,
                    Zip = _Instructor.Zip,
                    Phone = _Instructor.Phone,
                    CreatedBy = _Instructor.CreatedBy,
                    CreatedDate = _Instructor.CreatedDate,
                    ModifiedBy = _Instructor.ModifiedBy,
                    ModifiedDate = _Instructor.ModifiedDate
                };
                _context.Instructors.Add(_Instr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instr.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
