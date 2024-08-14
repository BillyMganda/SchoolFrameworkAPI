using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public class FormRepository : IFormRepository
    {
        ScoolFrameworkEntities _entities = new ScoolFrameworkEntities();
        public async Task CreateFormAsync(CreateFormRequest request)
        {
            var newForm = new Form
            {
                Name = request.Name,
                DateCreated = DateTime.UtcNow,
            };

            _entities.Form.Add(newForm);
            await _entities.SaveChangesAsync();
        }

        public async Task DeleteFormAsync(int id)
        {
            var formToDelete = await _entities.Form.FirstOrDefaultAsync(f => f.Id == id);

            if(formToDelete != null)
            {
                _entities.Form.Remove(formToDelete);
                await _entities.SaveChangesAsync();
            }
        }

        public async Task<GetFormResponse> GetFormByIdAsync(int id)
        {
            var form = await _entities.Form.FirstOrDefaultAsync(f => f.Id == id);

            var response = new GetFormResponse
            {
                Id = form.Id,
                Name = form.Name,
                DateCreated = form.DateCreated,
            };

            return response;
        }

        public async Task<IEnumerable<GetFormResponse>> GetFormsAsync()
        {
            var forms = await _entities.Form.ToListAsync();

            var response = forms.Select(f => new  GetFormResponse
            {
                Id = f.Id,
                Name = f.Name,
                DateCreated = f.DateCreated,
            }).ToList();

            return response;
        }

        public async Task UpdateFormAsync(UpdateFormRequest request)
        {
            var formToUpdate = await _entities.Form.FirstOrDefaultAsync(f => f.Id == request.Id);

            if (formToUpdate != null)
            {
                formToUpdate.Name = request.Name;

                await _entities.SaveChangesAsync();
            }
        }
    }
}