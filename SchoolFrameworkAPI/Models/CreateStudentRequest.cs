using System;

namespace SchoolFrameworkAPI.Models
{
    public class CreateStudentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ParentOrGuardianFirstName { get; set; }
        public string ParentOrGuardianLastName { get; set; }
        public string ParentOrGuardianPhoneNumber { get; set; }
        public string ParentOrGuardianEmailAddress { get; set; }
        public int FormId { get; set; }
    }
}