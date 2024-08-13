using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolFrameworkAPI.Controllers
{
    public class DepartmentsController : ApiController
    {
        [HttpGet]
        public IEnumerable<Department> GetDepartments()
        {
            using (ScoolFrameworkEntities _entities = new ScoolFrameworkEntities())
            {
                return _entities.Department.ToList();
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetDepartment(int id)
        {
            using (ScoolFrameworkEntities _entities = new ScoolFrameworkEntities())
            {
                var department = await _entities
                    .Department
                    .SingleOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {                    
                    return NotFound();
                }

                return Ok(department);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest("Department cannot be null");
            }

            using (ScoolFrameworkEntities _entities = new ScoolFrameworkEntities())
            {
                var newDepartment = new Department
                {
                    Name = department.Name,
                    DateCreated = DateTime.UtcNow,
                };

                _entities.Department.Add(newDepartment);
                await _entities.SaveChangesAsync();

                var departmentId = department.Id;
                var location = Url.Link("DefaultApi", new { id = departmentId });
                return Created(location, department);
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest("Department cannot be null");
            }

            using (ScoolFrameworkEntities _entities = new ScoolFrameworkEntities())
            {
                var existingDepartment = await _entities.Department.FindAsync(department.Id);

                if (existingDepartment == null)
                {
                    return NotFound();
                }

                existingDepartment.Name = department.Name;

                try
                {
                    await _entities.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {                    
                    return InternalServerError(ex);
                }
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteDepartment(int id)
        {
            using (ScoolFrameworkEntities _entities = new ScoolFrameworkEntities())
            {                
                var department = await _entities.Department.FindAsync(id);

                if (department == null)
                {
                    return NotFound();
                }
                
                _entities.Department.Remove(department);

                try
                {                    
                    await _entities.SaveChangesAsync();
                    return StatusCode(System.Net.HttpStatusCode.NoContent);
                }
                catch (Exception ex)
                {                    
                    return InternalServerError(ex);
                }
            }
        }
    }
}
