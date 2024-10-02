namespace ComplexToDo.Project.Models.Dtos
{
    public class ToDoListDto
    {
        public int Id { get; set; }
        public string ToDoListName { get; set; }

        public ICollection<ToDo> ToDos { get; set; }
    }
}
