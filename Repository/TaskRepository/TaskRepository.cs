
using Microsoft.EntityFrameworkCore;
using Zimozi.Assessment.Models;

namespace Zimozi.Assessment.Repository.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        public Context _context;
        public TaskRepository(Context context)
        {
            _context = context;
        }

        public async Task<Models.Task> CreateTask(Models.Task task)
        {
            var result = await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public Task<Models.Task> DeleteTask(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Models.Task>> GetByUserId(int userId)
        {
            return await _context.Tasks.Where((task) => task.UserId == userId).ToListAsync();
        }

        public async Task<Models.Task> GetDetailTaskById(int id)
        {
            return await _context.Tasks.Where((task) => task.Id == id).Include(task => task.TaskComments).Include(task => task.ChargedUser).FirstOrDefaultAsync(); 
        }
    
        public Task<Models.Task> UpdateTask(Models.Task task)
        {
            throw new NotImplementedException();
        }
    }
}
