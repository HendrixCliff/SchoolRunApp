namespace SchoolRunApp.API.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation
        public ICollection<StudentActivity> StudentActivities { get; set; } 
            = new List<StudentActivity>();
    }
}
