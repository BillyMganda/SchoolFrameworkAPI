using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolFrameworkAPI.Controllers
{
    public class TeachersController : ApiController
    {
        private readonly ITeacherRepository _repository;
        public TeachersController(ITeacherRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetTeachersAsync()
        {
            var result = await _repository.GetTeachersAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetTeacherByIdAsync(int id)
        {
            var result = await _repository.GetTeacherByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult>PostTeacherAsync([FromBody] CreateTeacherRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            await _repository.CreateTeacherAsync(request);

            var TeacherName = request.FirstName + " " + request.LastName;
            var location = Url.Link("DefaultApi", new { Name = TeacherName });
            return Created(location, request);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutTeacher([FromBody] UpdateTeacherRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var teacherToUpdate = await _repository.GetTeacherByIdAsync(request.Id);

            if (teacherToUpdate == null)
            {
                return NotFound();
            }

            await _repository.UpdateTeacherAsync(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTeacher(int id)
        {
            var teacher = await _repository.GetTeacherByIdAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            await _repository.DeleteTeacherAsync(id);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
