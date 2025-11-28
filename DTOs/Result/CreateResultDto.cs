namespace SchoolRunApp.API.DTOs.Result
{
    public class CreateResultDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }

        public double Score { get; set; }
        public string Term { get; set; }
        public string Session { get; set; }
    }
}
