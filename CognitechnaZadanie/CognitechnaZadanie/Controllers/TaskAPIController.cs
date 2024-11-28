using CognitechnaZadanie.Hubs;
using CognitechnaZadanie.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CognitechnaZadanie.Controllers;

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
        try
        {
            return Ok(await _dbContext.Get());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEntity>> GetTask(int id)
    {
        try
        {
            var task = await _dbContext.Get(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskEntity task)
    {
        if (id != task.Id)
        {
            return BadRequest("Id and task.Id are not equal.");
        }

        try
        {
            var taskResult = await _dbContext.Update(task);

            if (taskResult == null)
            {
                return NotFound();
            }

            await _hubContext.Clients.All.SendAsync("TaskUpdated", task);

            return Ok(taskResult);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public async Task<ActionResult<TaskEntity>> InsertTask(TaskEntity task)
    {
        try{
            var taskResult = await _dbContext.Insert(task);

            await _hubContext.Clients.All.SendAsync("TaskCreated", task);

            return taskResult;
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            await _dbContext.Delete(id);

            await _hubContext.Clients.All.SendAsync("TaskDeleted", id);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("process-heavy/{id}")]
    public async Task<IActionResult> ProcessHeavyTask(int id)
    {
        try
        {
            await Task.Factory.StartNew(() => Thread.Sleep(TimeSpan.FromSeconds(10)));

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}