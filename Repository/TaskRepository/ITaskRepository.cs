namespace Zimozi.Assessment.Repository.TaskRepository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> GetByUserId(int userId);
        Task<Models.Task> GetDetailTaskById(int id);
        Task<Models.Task> CreateTask(Models.Task task); 
        Task<Models.Task> UpdateTask(Models.Task task); 
        Task<Models.Task> DeleteTask(int id);
    }
}
