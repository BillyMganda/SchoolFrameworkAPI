using System;
using System.Collections.Generic;

namespace SchoolFrameworkAPI.Models
{
    public class GetFormStudentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public List<GetStudentResponse> StudentsList { get; set; }
    }
}