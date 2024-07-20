namespace TaskManagementSystem.Models.Domain
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }


        public Project Project { get; set; }

        public Employee AssignedTo { get; set; }

        public List<Note>? Notes { get; set; }
    }
}
