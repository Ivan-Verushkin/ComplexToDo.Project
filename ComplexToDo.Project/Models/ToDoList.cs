namespace ComplexToDo.Project.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public ICollection<ToDo> ToDos { get; set; }
        public string ToDoListName { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
