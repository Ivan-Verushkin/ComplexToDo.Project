using ComplexToDo.Project.Data;
using ComplexToDo.Project.Models;
using ComplexToDo.Project.Models.Dtos;
using ComplexToDo.Project.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ComplexToDo.Project.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ToDoListRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> CreateToDoItemByToDoListId(int toDoListId, DateTime? completeTillDate, string toDoText)
        {
            var toDoList = await _dbContext.ToDoLists.FirstOrDefaultAsync(x => x.Id == toDoListId);

            if (toDoList == null)
            {
                return 0;
            }

            var toDoItem = new ComplexToDo.Project.Models.ToDo() { ToDoListId = toDoListId, ToDoText = toDoText, CompleteTillDate = completeTillDate };

            await _dbContext.ToDos.AddAsync(toDoItem);

            await _dbContext.SaveChangesAsync();

            return toDoItem.Id;
        }

        public async Task<int> CreateToDoListByUserId(string id, string listName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return 0;
            }

            var toDoList = new ToDoList() { ToDoListName = listName, UserId = user.Id };

            await _dbContext.ToDoLists.AddAsync(toDoList);
            await _dbContext.SaveChangesAsync();

            return toDoList.Id;
        }

        public async Task<bool> DeleteToDoItemByToDoItemId(int toDoItemId)
        {
            var toDo = await _dbContext.ToDos.FirstOrDefaultAsync(x => x.Id == toDoItemId);
            if (toDo == null)
            {
                return false;
            }

            _dbContext.ToDos.Remove(toDo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteToDoListByToDoListId(int toDoListId)
        {
            var toDoList = await _dbContext.ToDoLists.FirstOrDefaultAsync(x => x.Id == toDoListId);
            if (toDoList == null)
            {
                return false;
            }

            _dbContext.ToDoLists.Remove(toDoList);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ToDoListDto>> GetAllToDoListsByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }

            var toDoLists = await _dbContext.ToDoLists
                .Where (t => t.UserId == user.Id)
                .Select(todo => new ToDoListDto
                {
                    Id = todo.Id,
                    ToDoListName = todo.ToDoListName,
                }).ToListAsync();

            foreach (var list  in toDoLists)
            {
                list.ToDos = await _dbContext.ToDos.Where(x => x.ToDoListId == list.Id).ToListAsync();
            }

            return toDoLists;
        }

        public async Task<ToDoList> GetToDoListById(int id)
        {
            var toDoList = await _dbContext.ToDoLists.FirstOrDefaultAsync(x => x.Id == id);
            if (toDoList == null) {
                return null;

            }
            return toDoList;
        }

        public async Task<ToDoList> GetToDoListByUserId(string id)
        {
            var toDoList = await _dbContext.ToDoLists.FirstOrDefaultAsync(x =>x.UserId == id);

            if (toDoList == null)
            {
                return null;
            }
            return toDoList;
        }

        public async Task<bool> UpdateToDoItemByToDoItemId(int toDoItemId, DateTime? completeTillDate, string toDoText)
        {
            var toDoItem = await _dbContext.ToDos.FirstOrDefaultAsync(x => x.Id == toDoItemId);

            if (toDoItem == null)
            {
                return false;
            }

            toDoItem.ToDoText = toDoText;
            toDoItem.CompleteTillDate = completeTillDate;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateToDoListByToDoListId(int toDoListId, string toDoListName)
        {
            var toDoList = await _dbContext.ToDoLists.FirstOrDefaultAsync(x => x.Id == toDoListId);

            if (toDoList == null)
            {
                return false;
            }

            _dbContext.ToDoLists.Remove(toDoList);

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
