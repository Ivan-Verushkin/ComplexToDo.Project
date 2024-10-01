namespace ComplexToDo.Project.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public DateTime? CompleteTillDate { get; set; }
        public string ToDoText { get; set; }
        public int ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }  // Navigation property}
    }
}
