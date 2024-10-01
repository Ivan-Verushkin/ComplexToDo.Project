using ComplexToDo.Project.Models;
using ComplexToDo.Project.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComplexToDo.Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoListRepository _toDoListRepository;

        public ToDoController(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        [HttpGet("todolists")]
        public async Task<IActionResult> GetAllToDoLists()
        {
            var email = GetUserEmailFromToken();

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid token, no email found.");
            }

            var res = await _toDoListRepository.GetAllToDoListsByEmail(email);

            if (res == null)
            {
                return NotFound("There are no todo lists for this user");
            }

            return Ok(res);
        }

        [HttpPost("create-todo-list")]
        public async Task<IActionResult> CreateToDoList([FromBody] ToDoListModel model)
        {
            var userId = GetUserIdFromToken();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token, no userId found.");
            }

            var res = await _toDoListRepository.CreateToDoListByUserId(userId, model.ToDoListName);

            if (res == 0)
            {
                return BadRequest("ToDo List was not created. There might be an issue with the user or the list.");
            }

            return Ok(new {Id = res});
        }

        [HttpPost("create-todo-item")]
        public async Task<IActionResult> CreateToDoItem([FromBody] ToDoItemModel model)
        {
            var userId = GetUserIdFromToken();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token, no user ID found.");
            }

            var res = await _toDoListRepository.CreateToDoItemByToDoListId(model.ToDoListId, model.CompleteTillDate, model.ToDoText);

            if (res == 0)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the ToDo list." });
            }

            return Ok(new { Id = res });
        }

        [HttpPut("update-todo-item/{id}")]
        public async Task<IActionResult> UpdateToDoItem([FromRoute] int id, [FromBody] UpdateToDoItemModel model)
        {
            var userId = GetUserIdFromToken();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token, no user ID found.");
            }

            var updated = await _toDoListRepository.UpdateToDoItemByToDoItemId(id, model.CompleteTillDate, model.ToDoText);

            if (!updated)
            {
                return NotFound( new {Message = "ToDo item not found" });
            }


            return Ok(new { Message = $"ToDo item with ID {id} updated successfully." });

        }

        [HttpPut("update-todo-list/{id}")]
        public async Task<IActionResult> UpdateToDoList([FromRoute] int id, [FromBody] UpdateToDoListModel model)
        {
            var userId = GetUserIdFromToken();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token, no user ID found.");
            }

            var toDoList = await _toDoListRepository.GetToDoListById(id);
            if (toDoList == null)
            {
                return NotFound(new { Message = "ToDo list not found." });
            }

            if (toDoList.UserId != userId)
            {
                return Forbid("You do not have permission to update this ToDo list.");
            }

            var updated = await _toDoListRepository.UpdateToDoListByToDoListId(id, model.ToDoListName);

            if (!updated)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the ToDo list." });
            }

            return Ok(new { Message = $"ToDo list with ID {id} updated successfully." });
        }

        [HttpDelete("delete-todo-item/{id}")]
        public async Task<IActionResult> DeleteToDoItem([FromRoute] int id)
        {
            var userId = GetUserIdFromToken();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token, no user ID found.");
            }

            var deleted = await _toDoListRepository.DeleteToDoItemByToDoItemId(id);

            if (!deleted)
            {
                return NotFound(new { Message = "Item not found" });
            }

            return Ok(new { Message = $"ToDo item with ID {id} deleted successfully." });
        }

        [HttpDelete("delete-todo-list/{id}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] int id)
        {
            var userId = GetUserIdFromToken();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token, no user ID found.");
            }

            var toDoList = await _toDoListRepository.GetToDoListById(id);
            if (toDoList == null)
            {
                return NotFound(new { Message = "ToDo list not found." });
            }

            if (toDoList.UserId != userId)
            {
                return Forbid("You do not have permission to delete this ToDo list.");
            }

            var deleted = await _toDoListRepository.DeleteToDoListByToDoListId(id);

            if (!deleted)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the ToDo list." });
            }

            return Ok(new { Message = $"ToDo list with ID {id} deleted successfully." });
        }


        private string GetUserEmailFromToken()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        private string GetUserIdFromToken()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
