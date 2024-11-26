using CognitechnaZadanie.Model;
using CognitechnaZadanie.Model.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CognitechnaZadanie.Hubs
{
    public class TasksHub : Hub
    {
        public async Task<IEnumerable<TaskEntity>> GetTasks()
        {
            using (TaskContext context = new TaskContext())
            {
                try
                {
                    return await context.Tasks.ToListAsync();
                }
                catch (Exception ex)
                {
                    return Enumerable.Empty<TaskEntity>();
                }
            }
        }

        public async Task SubmitTask(TaskEntity task)
        {
            using (TaskContext context = new TaskContext())
            {
                try
                {
                    context.Tasks.Add(task);
                    await context.SaveChangesAsync();
                    await Clients.All.SendAsync("TaskCreated", task);
                }
                catch (Exception ex)
                {
                    
                }
            }

        }

    }
}
