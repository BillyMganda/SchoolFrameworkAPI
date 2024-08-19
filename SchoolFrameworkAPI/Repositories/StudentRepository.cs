using DataAccessLayer;
using SchoolFrameworkAPI.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolFrameworkAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        ScoolFrameworkEntities _entities = new ScoolFrameworkEntities();

        public async Task CreateStudentAsync(CreateStudentRequest request)
        {
            var newStudent = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth.ToShortDateString(),
                ParentOrGuardianFirstName = request.ParentOrGuardianFirstName,
                ParentOrGuardianLastName = request.ParentOrGuardianLastName,
                ParentOrGuardianPhoneNumber = request.ParentOrGuardianPhoneNumber,
                ParentOrGuardianEmailAddress = request.ParentOrGuardianEmailAddress,
                FormId = request.FormId,
            };

            _entities.Student.Add(newStudent);
            await _entities.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var studentToDelete = await _entities.Student
                .FirstOrDefaultAsync(s => s.Id == id);

            if(studentToDelete != null)
            {
                _entities.Student.Remove(studentToDelete);
                await _entities.SaveChangesAsync();
            }
        }

        public async Task<GetStudentResponse> GetStudentByIdAsync(int id)
        {
            var student = await _entities.Student
                .Include(s => s.Form)
                .FirstOrDefaultAsync(s => s.Id == id);

            if(student != null)
            {
                var studentResponse = new GetStudentResponse
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    ParentOrGuardianFirstName = student.ParentOrGuardianFirstName,
                    ParentOrGuardianLastName = student.ParentOrGuardianLastName,
                    ParentOrGuardianPhoneNumber = student.ParentOrGuardianPhoneNumber,
                    ParentOrGuardianEmailAddress = student.ParentOrGuardianEmailAddress,
                    FormName = student.Form != null? student.Form.Name : null,
                };

                return studentResponse;
            }

            return null;
        }

        public async Task<IEnumerable<GetStudentResponse>> GetStudentsAsync()
        {
            var students = await _entities.Student
                .Include(s => s.Form)
                .ToListAsync();

            var studentResponses = students.Select(s => new GetStudentResponse
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                DateOfBirth = s.DateOfBirth,
                ParentOrGuardianFirstName = s.ParentOrGuardianFirstName,
                ParentOrGuardianLastName = s.ParentOrGuardianLastName,
                ParentOrGuardianPhoneNumber = s.ParentOrGuardianPhoneNumber,
                ParentOrGuardianEmailAddress = s.ParentOrGuardianEmailAddress,
                FormName = s.Form != null ? s.Form.Name : null,
            });

            return studentResponses;
        }

        public async Task UpdateStudentAsync(UpdateStudentRequest request)
        {
            var studentToUpdate = await _entities.Student
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            studentToUpdate.ParentOrGuardianFirstName = request.ParentOrGuardianFirstName;
            studentToUpdate.ParentOrGuardianLastName = request.ParentOrGuardianLastName;
            studentToUpdate.ParentOrGuardianPhoneNumber = request.ParentOrGuardianPhoneNumber;
            studentToUpdate.ParentOrGuardianEmailAddress = request.ParentOrGuardianEmailAddress;
            studentToUpdate.FormId = request.FormId;

            await _entities.SaveChangesAsync();
        }
    }
}