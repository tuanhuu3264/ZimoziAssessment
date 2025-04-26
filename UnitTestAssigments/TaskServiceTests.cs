using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using Zimozi.Assessment.BusinessModels.RequestModels.Response;
using Zimozi.Assessment.Models;
using Zimozi.Assessment.Repository.TaskRepository;
using Zimozi.Assessment.Services.TaskService;

namespace Test
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<ILogger<TaskService>> _mockLogger;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockLogger = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_mockTaskRepository.Object, _mockLogger.Object);
        }

        #region CreateTask Tests

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ValidModel_ReturnsSuccessResult()
        {
            var model = new CreateTaskModel
            {
                Name = "Test Task",
                Description = "Test Description",
                DeadlineDate = DateTime.Now.AddDays(1),
                UserId = 1
            };

            var createdTask = new Zimozi.Assessment.Models.Task
            {
                Id = 1,
                Name = model.Name,
                Description = model.Description,
                DeadlineDate = model.DeadlineDate,
                Status = Status.Pending,
                UserId = model.UserId,
                CreatedDate = DateTime.Now
            };

            _mockTaskRepository.Setup(repo => repo.CreateTask(It.IsAny<Zimozi.Assessment.Models.Task>()))
                .ReturnsAsync(createdTask);

            var result = await _taskService.CreateTask(model);

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Task created successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal(model.Name, result.Data.Name);
            Assert.Equal(model.Description, result.Data.Description);
            Assert.Equal(Status.Pending, result.Data.Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_EmptyTaskName_ReturnsBadRequestResult()
        {
            var model = new CreateTaskModel
            {
                Name = "",
                Description = "Test Description",
                DeadlineDate = DateTime.Now.AddDays(1),
                UserId = 1
            };

            var result = await _taskService.CreateTask(model);

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Task name is required", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NullTaskName_ReturnsBadRequestResult()
        {
            var model = new CreateTaskModel
            {
                Name = null,
                Description = "Test Description",
                DeadlineDate = DateTime.Now.AddDays(1),
                UserId = 1
            };

            var result = await _taskService.CreateTask(model);

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Task name is required", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_RepositoryFails_ReturnsErrorResult()
        {
            var model = new CreateTaskModel
            {
                Name = "Test Task",
                Description = "Test Description",
                DeadlineDate = DateTime.Now.AddDays(1),
                UserId = 1
            };

            _mockTaskRepository.Setup(repo => repo.CreateTask(It.IsAny<Zimozi.Assessment.Models.Task>()))
                .ReturnsAsync((Zimozi.Assessment.Models.Task)null);

            var result = await _taskService.CreateTask(model);

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Failed to create task", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ThrowsException_ReturnsErrorResult()
        {
            var model = new CreateTaskModel
            {
                Name = "Test Task",
                Description = "Test Description",
                DeadlineDate = DateTime.Now.AddDays(1),
                UserId = 1
            };

            _mockTaskRepository.Setup(repo => repo.CreateTask(It.IsAny<Zimozi.Assessment.Models.Task>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _taskService.CreateTask(model);

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("An error occurred while creating the task", result.Message);
            Assert.Null(result.Data);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public async System.Threading.Tasks.Task GetById_ExistingTask_ReturnsSuccessResult()
        {
            var taskId = 1;
            var task = new Zimozi.Assessment.Models.Task
            {
                Id = taskId,
                Name = "Test Task",
                Description = "Test Description",
                Status = Status.Pending,
                CreatedDate = DateTime.Now,
                DeadlineDate = DateTime.Now.AddDays(1),
                UserId = 1,
                TaskComments = new List<Zimozi.Assessment.Models.TaskComment>
                {
                    new Zimozi.Assessment.Models.TaskComment
                    {
                        Id = 1,
                        Content = "Test Comment",
                        TaskId = taskId,
                        UserId = 1,
                        CreatedAt = DateTime.Now
                    }
                },
                ChargedUser = new Zimozi.Assessment.Models.User
                {
                    Id = 1,
                    Name = "Test User",
                    Email = "test@example.com",
                    Role = Role.User
                }
            };

            _mockTaskRepository.Setup(repo => repo.GetDetailTaskById(taskId))
                .ReturnsAsync(task);

            var result = await _taskService.GetById(taskId);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Task found successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(taskId, result.Data.Id);
            Assert.Equal("Test Task", result.Data.Name);
            Assert.Equal("Test Description", result.Data.Description);
            Assert.Equal(Status.Pending, result.Data.Status);
            Assert.NotNull(result.Data.Comments);
            Assert.Single(result.Data.Comments);
            Assert.Equal("Test Comment", result.Data.Comments.First().Content);
            Assert.NotNull(result.Data.userResponse);
            Assert.Equal(1, result.Data.userResponse.Id);
            Assert.Equal("Test User", result.Data.userResponse.Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_NonExistingTask_ReturnsNotFoundResult()
        {
            var taskId = 999;
            _mockTaskRepository.Setup(repo => repo.GetDetailTaskById(taskId))
                .ReturnsAsync((Zimozi.Assessment.Models.Task)null);

            var result = await _taskService.GetById(taskId);

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Task not found", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_ThrowsException_ReturnsErrorResult()
        {
            var taskId = 1;
            _mockTaskRepository.Setup(repo => repo.GetDetailTaskById(taskId))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _taskService.GetById(taskId);

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("An error occurred while retrieving the task", result.Message);
            Assert.Null(result.Data);
        }

        #endregion

        #region GetByUserId Tests

        [Fact]
        public async System.Threading.Tasks.Task GetByUserId_TasksExist_ReturnsSuccessResult()
        {
            var userId = 1;
            var tasks = new List<Zimozi.Assessment.Models.Task>
            {
                new Zimozi.Assessment.Models.Task
                {
                    Id = 1,
                    Name = "Task 1",
                    Description = "Description 1",
                    Status = Status.Pending,
                    CreatedDate = DateTime.Now,
                    DeadlineDate = DateTime.Now.AddDays(1),
                    UserId = userId
                },
                new Zimozi.Assessment.Models.Task
                {
                    Id = 2,
                    Name = "Task 2",
                    Description = "Description 2",
                    Status = Status.Pending,
                    CreatedDate = DateTime.Now,
                    DeadlineDate = DateTime.Now.AddDays(2),
                    UserId = userId
                }
            };

            _mockTaskRepository.Setup(repo => repo.GetByUserId(userId))
                .ReturnsAsync(tasks);

            var result = await _taskService.GetByUserId(userId);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Tasks found successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Task 1", result.Data[0].Name);
            Assert.Equal("Task 2", result.Data[1].Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByUserId_NoTasks_ReturnsEmptyListResult()
        {
            var userId = 1;
            _mockTaskRepository.Setup(repo => repo.GetByUserId(userId))
                .ReturnsAsync(new List<Zimozi.Assessment.Models.Task>());


            var result = await _taskService.GetByUserId(userId);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("No tasks found for this user", result.Message);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByUserId_NullResponse_ReturnsEmptyListResult()
        {

            var userId = 1;
            _mockTaskRepository.Setup(repo => repo.GetByUserId(userId))
                .ReturnsAsync((List<Zimozi.Assessment.Models.Task>)null);

            var result = await _taskService.GetByUserId(userId);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("No tasks found for this user", result.Message);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByUserId_ThrowsException_ReturnsErrorResult()
        {
            var userId = 1;
            _mockTaskRepository.Setup(repo => repo.GetByUserId(userId))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _taskService.GetByUserId(userId);

            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("An error occurred while retrieving tasks", result.Message);
            Assert.Null(result.Data);
        }

        #endregion
    }
}