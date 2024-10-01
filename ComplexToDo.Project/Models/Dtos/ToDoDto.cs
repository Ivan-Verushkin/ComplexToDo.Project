namespace ComplexToDo.Project.Models.Dtos
{
    public class ToDoDto
    {
        public int Id { get; set; }
        public DateTime? CompleteTillDate { get; set; }
        public string ToDoText { get; set; }
    }
}
