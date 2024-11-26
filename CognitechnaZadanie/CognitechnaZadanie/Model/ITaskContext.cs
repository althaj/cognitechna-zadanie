using CognitechnaZadanie.Model.Entities;

public interface ITaskContext
{
    public Task<IEnumerable<TaskEntity>> Get();

    public Task<TaskEntity?> Get(int id);

    public Task<TaskEntity> Insert(TaskEntity task);

    public Task<TaskEntity?> Update(TaskEntity task);

    public Task Delete(int id);
}