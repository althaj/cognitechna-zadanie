using CognitechnaZadanie.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CognitechnaZadanie.Model
{
    public class TaskContext : DbContext, ITaskContext
    {
        public async Task Delete(int id)
        {
            TaskEntity? task = await Get(id);
            if(task != null)
            {
                Set<TaskEntity>().Remove(task);
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskEntity>> Get()
        {
            return await Set<TaskEntity>().ToListAsync();
        }

        public async Task<TaskEntity?> Get(int id)
        {
            return await Set<TaskEntity>().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskEntity> Insert(TaskEntity task)
        {
            Set<TaskEntity>().Add(task);
            await SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity?> Update(TaskEntity task)
        {
            TaskEntity? trackedTask = await Get(task.Id);
            if(trackedTask != null)
            {
                trackedTask.Title = task.Title;
                trackedTask.Description = task.Description;
                await SaveChangesAsync();
                return trackedTask;
            }
            return null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("TaskDatabase");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity(typeof(TaskEntity));
    }
}
