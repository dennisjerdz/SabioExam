namespace SabioExam.DATA.Entities
{
    public class Job
    {
        public long JobId { get; set; }
        public required byte JobType { get; set; }
        public required string JobParameters { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
