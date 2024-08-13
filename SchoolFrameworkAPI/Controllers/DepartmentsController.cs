using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolFrameworkAPI.Controllers
{
    public class DepartmentsController : ApiController
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentsController(IDepartmentRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetDepartments()
        {
            var result = await _repository.GetDepartmentsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetDepartment(int id)
        {            
            var department = await _repository.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostDepartment([FromBody] CreateDepartmentRequest department)
        {
            if (department == null)
            {
                return BadRequest("Department cannot be null");
            }

            await _repository.CreateDepartmentAsync(department);

            var departmentName = department.Name;
            var location = Url.Link("DefaultApi", new { Namw = departmentName });
            return Created(location, department);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutDepartment([FromBody] UpdateDepartmentRequest department)
        {
            if (department == null)
            {
                return BadRequest("Department cannot be null");
            }

            var departmentToUpdate = await _repository.GetDepartmentByIdAsync(department.Id);

            if (departmentToUpdate == null)
            {
                return NotFound();
            }

            await _repository.UpdateDepartmentAsync(department);

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteDepartment(int id)
        {
            var department = await _repository.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            await _repository.DeleteDepartmentAsync(id);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
