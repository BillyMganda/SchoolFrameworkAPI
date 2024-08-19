using SchoolFrameworkAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<GetStudentResponse>> GetStudentsAsync();
        Task<GetStudentResponse> GetStudentByIdAsync(int id);
        Task CreateStudentAsync(CreateStudentRequest request);
        Task UpdateStudentAsync(UpdateStudentRequest request);
        Task DeleteStudentAsync(int id);
    }
}
