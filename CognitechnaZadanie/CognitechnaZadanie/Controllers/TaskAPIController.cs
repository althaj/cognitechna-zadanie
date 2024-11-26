using CognitechnaZadanie.Hubs;
using CognitechnaZadanie.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace TodoApi.Controllers;

[Route("api/tasks")]
[ApiController]
public class TaskAPIController : ControllerBase
{
    private readonly ITaskContext _dbContext;
    private readonly IHubContext<TasksHub> _hubContext;

    public TaskAPIController(ITaskContext dbContext, IHubContext<TasksHub> hubContext)
    {
        _dbContext = dbContext;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks()
    {
        return Ok(await _dbContext.Get());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEntity>> GetTask(int id)
    {
        var task = await _dbContext.Get(id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskEntity task)
    {
        if (id != task.Id)
        {
            return BadRequest();
        }

        var taskResult = await _dbContext.Update(task);

        if (taskResult == null)
        {
            return NotFound();
        }

        return Ok(taskResult);
    }
    
    [HttpPost]
    public async Task<ActionResult<TaskEntity>> InsertTask(TaskEntity task)
    {
        var taskResult = await _dbContext.Insert(task);

        await _hubContext.Clients.All.SendAsync("TaskCreated", task);

        return taskResult; 
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        await _dbContext.Delete(id);
        return Ok();
    }
}