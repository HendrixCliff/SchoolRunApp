using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Models
{
    public class Result
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public StudentProfile Student { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public int ClassId { get; set; }        
    public Class Class { get; set; }      
    public double Score { get; set; }
    public string Term { get; set; }
    public string Session { get; set; }
}

}
