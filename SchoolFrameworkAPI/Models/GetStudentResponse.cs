namespace SchoolFrameworkAPI.Models
{
    public class GetStudentResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string ParentOrGuardianFirstName { get; set; }
        public string ParentOrGuardianLastName { get; set; }
        public string ParentOrGuardianPhoneNumber { get; set; }
        public string ParentOrGuardianEmailAddress { get; set; }
        public string FormName { get; set; }
    }
}