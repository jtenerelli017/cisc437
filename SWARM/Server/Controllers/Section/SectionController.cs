using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Sct
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : Controller
    {
        protected readonly SWARMOracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetSection/{pSectionId}")]
        public async Task<IActionResult> GetSection(int pSectionId)
        {
            Section itmSection = await _context.Sections
                .Where(
                    x => x.SectionId == pSectionId
                ).FirstOrDefaultAsync();

            return Ok(itmSection);
        }

        [HttpDelete]
        [Route("DeleteSection/{pSectionId}")]
        public async Task<IActionResult> DeleteSection(int pSectionId)
        {
            Section itmSection = await _context.Sections
                .Where(
                    x => x.SectionId == pSectionId
                ).FirstOrDefaultAsync();
            _context.Remove(itmSection);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Section)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sct = await _context.Sections
                    .Where(
                        x => x.SectionId == _Section.SectionId
                    ).FirstOrDefaultAsync();
                if (_Sct == null)
                {
                    bExist = false;
                    _Sct = new Section();
                }
                else
                    bExist = true;

                _Sct.SectionId = _Section.SectionId;
                _Sct.CourseNo = _Section.CourseNo;
                _Sct.SectionNo = _Section.SectionNo;
                _Sct.StartDateTime = _Section.StartDateTime;
                _Sct.Location = _Section.Location;
                _Sct.InstructorId = _Section.InstructorId;
                _Sct.Capacity = _Section.Capacity;
                _Sct.CreatedBy = _Section.CreatedBy;
                _Sct.CreatedDate = _Section.CreatedDate;
                _Sct.ModifiedBy = _Section.ModifiedBy;
                _Sct.ModifiedDate = _Section.ModifiedDate;
                _Sct.SchoolId = _Section.SchoolId;
                if (bExist)
                    _context.Sections.Update(_Sct);
                else
                    _context.Sections.Add(_Sct);
                
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Sct.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Section)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sct = await _context.Sections
                    .Where(
                        x => x.SectionId == _Section.SectionId
                    ).FirstOrDefaultAsync();
                if (_Sct != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record already exists");
                _Sct = new Section
                {
                    SectionId = _Section.SectionId,
                    CourseNo = _Section.CourseNo,
                    SectionNo = _Section.SectionNo,
                    StartDateTime = _Section.StartDateTime,
                    Location = _Section.Location,
                    InstructorId = _Section.InstructorId,
                    Capacity = _Section.Capacity,
                    CreatedBy = _Section.CreatedBy,
                    CreatedDate = _Section.CreatedDate,
                    ModifiedBy = _Section.ModifiedBy,
                    ModifiedDate = _Section.ModifiedDate,
                    SchoolId = _Section.SchoolId
                };
                _context.Sections.Add(_Sct);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Sct.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
