using System;


namespace SchoolRunApp.API.DTOs.Announcement
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; }
        public string PostedBy { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public string? ClassName { get; set; }
        public string? SubjectName { get; set; }
        public string? StudentName { get; set; }
    }
}

