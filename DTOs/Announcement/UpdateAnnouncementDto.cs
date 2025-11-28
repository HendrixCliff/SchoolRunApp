namespace SchoolRunApp.API.DTOs.Announcement
{
    public class UpdateAnnouncementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public string? Type { get; set; }  
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? StudentId { get; set; }
    }
}
