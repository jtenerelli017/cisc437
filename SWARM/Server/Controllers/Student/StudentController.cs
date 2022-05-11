using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Stdnt
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetStudent/{pStudentId}")]
        public async Task<IActionResult> GetStudent(int pStudentId)
        {
            Student itmStudent = await _context.Students
                .Where(
                    x => x.StudentId == pStudentId
                ).FirstOrDefaultAsync();

            return Ok(itmStudent);
        }

        [HttpDelete]
        [Route("DeleteStudent/{pStudentId}")]
        public async Task<IActionResult> DeleteStudent(int pStudentId)
        {
            Student itmStudent = await _context.Students
                .Where(
                    x => x.StudentId == pStudentId
                ).FirstOrDefaultAsync();
            _context.Remove(itmStudent);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student _Student)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stdnt = await _context.Students
                    .Where(
                        x => x.StudentId == _Student.StudentId
                    ).FirstOrDefaultAsync();
                if (_Stdnt == null)
                {
                    bExist = false;
                    _Stdnt = new Student();
                }
                else
                    bExist = true;

                _Stdnt.StudentId = _Student.StudentId;
                _Stdnt.Salutation = _Student.Salutation;
                _Stdnt.FirstName = _Student.FirstName;
                _Stdnt.LastName = _Student.LastName;
                _Stdnt.StreetAddress = _Student.StreetAddress;
                _Stdnt.Zip = _Student.Zip;
                _Stdnt.Phone = _Student.Phone;
                _Stdnt.Employer = _Student.Employer;
                _Stdnt.RegistrationDate = _Student.RegistrationDate;
                _Stdnt.CreatedBy = _Student.CreatedBy;
                _Stdnt.CreatedDate = _Student.CreatedDate;
                _Stdnt.ModifiedBy = _Student.ModifiedBy;
                _Stdnt.ModifiedDate = _Student.ModifiedDate;
                _Stdnt.SchoolId = _Student.SchoolId;
                if (bExist)
                    _context.Students.Update(_Stdnt);
                else
                    _context.Students.Add(_Stdnt);
                
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Stdnt.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Student)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stdnt = await _context.Students
                    .Where(
                        x => x.StudentId == _Student.StudentId
                    ).FirstOrDefaultAsync();
                if (_Stdnt != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Stdnt = new Student
                {
                    StudentId = _Student.StudentId,
                    Salutation = _Student.Salutation,
                    FirstName = _Student.FirstName,
                    LastName = _Student.LastName,
                    StreetAddress = _Student.StreetAddress,
                    Zip = _Student.Zip,
                    Phone = _Student.Phone,
                    Employer = _Student.Employer,
                    RegistrationDate = _Student.RegistrationDate,
                    CreatedBy = _Student.CreatedBy,
                    CreatedDate = _Student.CreatedDate,
                    ModifiedBy = _Student.ModifiedBy,
                    ModifiedDate = _Student.ModifiedDate,
                    SchoolId = _Student.SchoolId
                };
                _context.Students.Add(_Stdnt);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Stdnt.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
