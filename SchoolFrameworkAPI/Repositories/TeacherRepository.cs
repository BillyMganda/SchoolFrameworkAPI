using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        ScoolFrameworkEntities _entities = new ScoolFrameworkEntities();
        public async Task<IEnumerable<GetTeacherResponse>> GetTeachersAsync()
        {
            var teachers = await _entities.Teacher.ToListAsync();

            var response = teachers.Select(t => new  GetTeacherResponse
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                MobileNumber = t.MobileNumber,
                EmailAddress = t.EmailAddress,
                DepartmentId = t.DepartmentId,
                DateCreated = t.DateCreated,
            }).ToList();

            return response;
        }

        public async Task<GetTeacherResponse> GetTeacherByIdAsync(int id)
        {
            var teacher = await _entities
                .Teacher
                .FirstOrDefaultAsync(d => d.Id == id);

            var response = new GetTeacherResponse
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MobileNumber = teacher.MobileNumber,
                EmailAddress = teacher.EmailAddress,
                DepartmentId = teacher.DepartmentId,
                DateCreated = teacher.DateCreated,
            };

            return response;
        }

        public async Task CreateTeacherAsync(CreateTeacherRequest request)
        {
            var newTeacher = new Teacher
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MobileNumber = request.MobileNumber,
                EmailAddress = request.EmailAddress,
                DepartmentId = request.DepartmentId,
                DateCreated = DateTime.UtcNow,
            };

            _entities.Teacher.Add(newTeacher);
            await _entities.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(UpdateTeacherRequest request)
        {
            var teacherToUpdate = await _entities
                .Teacher
                .FirstOrDefaultAsync(d => d.Id == request.Id);

            if (teacherToUpdate != null)
            {
                teacherToUpdate.FirstName = request.FirstName;
                teacherToUpdate.LastName = request.LastName;
                teacherToUpdate.MobileNumber = request.MobileNumber;
                teacherToUpdate.EmailAddress = request.EmailAddress;

                await _entities.SaveChangesAsync();
            }
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacherToDelete = await _entities
                .Teacher
                .FirstOrDefaultAsync(d => d.Id == id);

            if (teacherToDelete != null)
            {
                _entities.Teacher.Remove(teacherToDelete);
                await _entities.SaveChangesAsync();
            }
        }
    }
}