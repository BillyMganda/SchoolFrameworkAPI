using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolFrameworkAPI.Controllers
{
    public class StudentsController : ApiController
    {
        private readonly IStudentRepository _repository;
        public StudentsController(IStudentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetStudents()
        {
            var result = await _repository.GetStudentsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetStudent(int id)
        {
            var student = await _repository.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostStudent([FromBody] CreateStudentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Student cannot be null");
            }

            await _repository.CreateStudentAsync(request);

            var StudentFirstName = request.FirstName;
            var location = Url.Link("DefaultApi", new { FirstName = StudentFirstName });
            return Created(location, request);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutStudent([FromBody] UpdateStudentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Student cannot be null");
            }

            var studentToUpdate = await _repository.GetStudentByIdAsync(request.Id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            await _repository.UpdateStudentAsync(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteStudent(int id)
        {
            var student = await _repository.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            await _repository.DeleteStudentAsync(id);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
