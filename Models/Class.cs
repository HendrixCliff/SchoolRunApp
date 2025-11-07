using System.Collections.Generic;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public ICollection<StudentProfile> Students { get; set; } = new List<StudentProfile>();
    }
}
