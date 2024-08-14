using System;

namespace SchoolFrameworkAPI.Models
{
    public class GetTeacherResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int DepartmentId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}