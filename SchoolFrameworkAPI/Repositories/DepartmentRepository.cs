using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        ScoolFrameworkEntities _entities = new ScoolFrameworkEntities();

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            var departments = await _entities.Department.ToListAsync();
            return departments;
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            var department = await _entities
                .Department
                .FirstOrDefaultAsync(d => d.Id == id);

            return department;
        }
        public async Task CreateDepartmentAsync(CreateDepartmentRequest request)
        {
            var newDepartment = new Department
            {
                Name = request.Name,
                DateCreated = DateTime.UtcNow,
            };

            _entities.Department.Add(newDepartment);
            await _entities.SaveChangesAsync();
        }

        public async Task UpdateDepartmentAsync(UpdateDepartmentRequest request)
        {
            var departmentToUpdate = await _entities
                .Department
                .FirstOrDefaultAsync(d => d.Id == request.Id);

            if (departmentToUpdate != null)
            {
                departmentToUpdate.Name = request.Name;

                await _entities.SaveChangesAsync();
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var departmentToDelete = await _entities
                .Department
                .FirstOrDefaultAsync(d => d.Id == id);

            if(departmentToDelete != null)
            {
                _entities.Department.Remove(departmentToDelete);
                await _entities.SaveChangesAsync();
            }
        }
    }
}