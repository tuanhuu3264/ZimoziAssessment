using Zimozi.Assessment.BusinessModels.RequestModels.Response;
using Zimozi.Assessment.BusinessModels.ResponseModels;
using Zimozi.Assessment.BusinessModels.ResponseModels.Task;

namespace Zimozi.Assessment.Services.TaskService
{
    public interface ITaskService
    {

        public Task<IBusinessResult<List<TaskResponse>>> GetByUserId(int userId);
        public Task<IBusinessResult<TaskResponse>> GetById(int id);

        public Task<IBusinessResult<TaskResponse>> CreateTask(CreateTaskModel model);

    }
}
