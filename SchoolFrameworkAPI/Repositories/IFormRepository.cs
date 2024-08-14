using SchoolFrameworkAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public interface IFormRepository
    {
        Task<IEnumerable<GetFormResponse>> GetFormsAsync();
        Task<GetFormResponse> GetFormByIdAsync(int id);
        Task CreateFormAsync(CreateFormRequest request);
        Task UpdateFormAsync(UpdateFormRequest request);
        Task DeleteFormAsync(int id);
    }
}
