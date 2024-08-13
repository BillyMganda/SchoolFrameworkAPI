using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetTeachersAsync();
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task CreateTeacherAsync(CreateTeacherRequest request);
        Task UpdateTeacherAsync(UpdateTeacherRequest request);
        Task DeleteTeacherAsync(int id);
    }
}
