using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task CreateDepartmentAsync(CreateDepartmentRequest request);
        Task UpdateDepartmentAsync(UpdateDepartmentRequest request);
        Task DeleteDepartmentAsync(int id);
    }
}
