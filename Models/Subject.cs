using System.Collections.Generic;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
