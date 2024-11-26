using CognitechnaZadanie.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CognitechnaZadanie.Model
{
    public class TaskContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("TaskDatabase");
    }
}
