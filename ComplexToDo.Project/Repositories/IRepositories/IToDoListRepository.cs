using ComplexToDo.Project.Models;
using ComplexToDo.Project.Models.Dtos;

namespace ComplexToDo.Project.Repositories.IRepositories
{
    public interface IToDoListRepository
    {
        Task<List<ToDoDto>> GetAllToDoListsByEmail(string email); 

        Task<ToDoList> GetToDoListByUserId (string id);

        Task<ToDoList> GetToDoListById (int id);

        Task<int> CreateToDoListByUserId(string id, string listName);

        Task<int> CreateToDoItemByToDoListId(int toDoListId, DateTime? completeTillDate, string toDoText);
        Task<bool> UpdateToDoItemByToDoItemId(int toDoItemId, DateTime? completeTillDate, string toDoText);

        Task<bool> UpdateToDoListByToDoListId(int toDoListId, string toDoListName);

        Task<bool> DeleteToDoItemByToDoItemId(int toDoItemId);
        Task<bool> DeleteToDoListByToDoListId(int toDoListId);
    }
}
