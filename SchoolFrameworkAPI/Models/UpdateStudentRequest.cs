namespace SchoolFrameworkAPI.Models
{
    public class UpdateStudentRequest
    {
        public int Id { get; set; }        
        public string ParentOrGuardianFirstName { get; set; }
        public string ParentOrGuardianLastName { get; set; }
        public string ParentOrGuardianPhoneNumber { get; set; }
        public string ParentOrGuardianEmailAddress { get; set; }
        public int FormId { get; set; }
    }
}