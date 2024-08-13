namespace SchoolFrameworkAPI.Models
{
    public class CreateTeacherRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int DepartmentId { get; set; }
    }
}