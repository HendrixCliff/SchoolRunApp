namespace SchoolRunApp.API.DTOs
{
    public class ResultDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public double Score { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Session { get; set; } = string.Empty;
    }
}
