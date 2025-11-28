namespace SchoolRunApp.API.DTOs.Announcement
{
    public class CreateAnnouncementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public int PostedByUserId { get; set; }

        public string Type { get; set; } = "General"; 
        // options: "General" or "Targeted"

        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? StudentId { get; set; }
    }
}
