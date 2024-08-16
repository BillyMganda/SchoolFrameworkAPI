using SchoolFrameworkAPI.Models;
using SchoolFrameworkAPI.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolFrameworkAPI.Controllers
{
    public class FormsController : ApiController
    {
        private readonly IFormRepository _repository;
        public FormsController(IFormRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetForms()
        {
            var result = await _repository.GetFormsAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetForm(int id)
        {
            var result = await _repository.GetFormByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost]
        public async Task<IHttpActionResult> PostForm([FromBody] CreateFormRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            await _repository.CreateFormAsync(request);

            var formName = request.Name;
            var location = Url.Link("DefaultApi", new { Name = formName });
            return Created(location, request);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutForm([FromBody] UpdateFormRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var formToUpdate = await _repository.GetFormByIdAsync(request.Id);

            if (formToUpdate == null)
            {
                return NotFound();
            }

            await _repository.UpdateFormAsync(request);

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteForm(int id)
        {
            try
            {
                var formToDelete = await _repository.GetFormByIdAsync(id);

                if (formToDelete == null)
                {
                    return NotFound();
                }

                await _repository.DeleteFormAsync(id);

                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }            
        }
    }
}
