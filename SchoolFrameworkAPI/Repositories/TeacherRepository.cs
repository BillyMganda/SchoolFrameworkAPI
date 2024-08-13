using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        ScoolFrameworkEntities _entities = new ScoolFrameworkEntities();
        public async Task<IEnumerable<Teacher>> GetTeachersAsync()
        {
            var teachers = await _entities.Teacher.ToListAsync();
            return teachers;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            var teacher = await _entities
                .Teacher
                .FirstOrDefaultAsync(d => d.Id == id);

            return teacher;
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