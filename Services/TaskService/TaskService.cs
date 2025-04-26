using Zimozi.Assessment.BusinessModels.RequestModels.Response;
using Zimozi.Assessment.BusinessModels.ResponseModels;
using Zimozi.Assessment.BusinessModels.ResponseModels.Task;
using Zimozi.Assessment.Repository.TaskRepository;

namespace Zimozi.Assessment.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<IBusinessResult<TaskResponse>> CreateTask(CreateTaskModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    _logger.LogWarning($"{DateTime.Now} - TaskService - CreateTask - Task name is required");
                    return new BusinessResult<TaskResponse>
                    {
                        StatusCode = 400,
                        Message = "Task name is required",
                        Data = null
                    };
                }

                var newTask = new Models.Task()
                {
                    CompletedDate = null,
                    CreatedDate = DateTime.Now,
                    DeadlineDate = model.DeadlineDate,
                    Description = model.Description,
                    Name = model.Name,
                    Status = Models.Status.Pending,
                    UserId = model.UserId,
                };

                var createdTask = await _taskRepository.CreateTask(newTask);

                if (createdTask == null)
                {
                    _logger.LogError($"{DateTime.Now} - TaskService - CreateTask - Failed to create task in repository");
                    return new BusinessResult<TaskResponse>
                    {
                        StatusCode = 500,
                        Message = "Failed to create task",
                        Data = null
                    };
                }

                _logger.LogInformation($"{DateTime.Now} - TaskService - CreateTask - Successfully created task with ID: {createdTask.Id}");

                return new BusinessResult<TaskResponse>
                {
                    StatusCode = 201,
                    Message = "Task created successfully",
                    Data = new TaskResponse()
                    {
                        Id = createdTask.Id,
                        Name = createdTask.Name,
                        Description = createdTask.Description,
                        Status = createdTask.Status,
                        CreatedDate = createdTask.CreatedDate,
                        DeadlineDate = createdTask.DeadlineDate,
                        CompletedDate = createdTask.CompletedDate,
                        UserId = createdTask.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - TaskService - CreateTask - Error: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");

                return new BusinessResult<TaskResponse>
                {
                    StatusCode = 500,
                    Message = "An error occurred while creating the task: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IBusinessResult<TaskResponse>> GetById(int id)
        {
            try
            {
                var task = await _taskRepository.GetDetailTaskById(id);
                
                if (task == null)
                {
                    _logger.LogWarning($"{DateTime.Now} - TaskService - GetById - Task not found with ID: {id}");
                    return new BusinessResult<TaskResponse>
                    {
                        StatusCode = 404,
                        Message = "Task not found",
                        Data = null
                    };
                } 

                _logger.LogInformation($"{DateTime.Now} - TaskService - GetById - Successfully retrieved task with ID: {id}");

                return new BusinessResult<TaskResponse>
                {
                    StatusCode = 200,
                    Message = "Task found successfully",
                    Data = new TaskResponse()
                    {
                        Comments = task.TaskComments?.Select(x => new Comment()
                        {
                            Content = x.Content,
                            CreatedAt = x.CreatedAt,
                            Id = x.Id,
                            TaskId = x.TaskId,
                            UserId = x.UserId,
                        }).ToList() ?? new List<Comment>(),
                        userResponse = task.ChargedUser != null ? new BusinessModels.ResponseModels.User.UserResponse()
                        {
                            Id = task.ChargedUser.Id,
                            Email = task.ChargedUser.Email,
                            Name = task.ChargedUser.Name,
                            Role = task.ChargedUser.Role
                        } : null,
                        CompletedDate = task.CompletedDate,
                        Name = task.Name,
                        Id = task.Id,
                        DeadlineDate = task.DeadlineDate,
                        CreatedDate = task.CreatedDate,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - TaskService - GetById - Error: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");

                return new BusinessResult<TaskResponse>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving the task: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IBusinessResult<List<TaskResponse>>> GetByUserId(int userId)
        {
            try
            {
                var tasks = await _taskRepository.GetByUserId(userId);

                if (tasks == null || !tasks.Any())
                {
                    _logger.LogInformation($"{DateTime.Now} - TaskService - GetByUserId - No tasks found for user ID: {userId}");
                    return new BusinessResult<List<TaskResponse>>
                    {
                        StatusCode = 200,
                        Message = "No tasks found for this user",
                        Data = new List<TaskResponse>()
                    };
                }

                _logger.LogInformation($"{DateTime.Now} - TaskService - GetByUserId - Found {tasks.Count()} tasks for user ID: {userId}");

                return new BusinessResult<List<TaskResponse>>
                {
                    StatusCode = 200,
                    Message = "Tasks found successfully",
                    Data = tasks.Select(task =>
                        new TaskResponse()
                        {
                            Id = task.Id,
                            UserId = task.UserId,
                            CreatedDate = task.CreatedDate,
                            Description = task.Description,
                            Status = task.Status,
                            DeadlineDate = task.DeadlineDate,
                            Name = task.Name,
                            CompletedDate = task.CompletedDate,
                        }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - TaskService - GetByUserId - Error: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");

                return new BusinessResult<List<TaskResponse>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving tasks: " + ex.Message,
                    Data = null
                };
            }
        }
    }
}
