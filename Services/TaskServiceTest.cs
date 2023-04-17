using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Models;
using TaskAPI.Repository;
using TaskAPI.Services;
using TaskAPI.Task.Contracts.Queries;

namespace TaskAPI.Test.Services
{
    public class TaskServiceTest
    {
        private readonly ITaskService taskService;
        private readonly Mock<ITaskRepository> _mockTaskRepo;

        public TaskServiceTest()
        {
            _mockTaskRepo = new Mock<ITaskRepository>();
            taskService = new TaskService(_mockTaskRepo.Object);
        }

        [Fact]
        public void CreateTest()
        {
            TaskInformation taskInformation = new TaskInformation() { TaskId = 1, Title = "Move bags", Description = "Move bags", Team = "Team2", AssigneeName = new HashSet<string>(new string[] { "one", "one", "two", "two", "three", "three" }), CreatedDate = DateTime.Now, DueDate = DateTime.Today };
            Tasks task = new Tasks() { TaskId = 1, Title = "Move bags", Description = "Move bags", Team = "Team2", CreatedDate = DateTime.Now, DueDate = DateTime.Today };
            _mockTaskRepo.Setup(repo => repo.GetTaskData(task))
                .Returns(true);

            _mockTaskRepo.Setup(repo => repo.SaveCreateTask(task));

            var result = taskService.CreateTask(task);

            var viewResult = Assert.IsType<bool>(result);

            Assert.True(result);

        }
        [Fact]
        public void GetTask()
        {
            GetAllPostQuery query = new GetAllPostQuery() { assigneeName = "Shikha", team = "team1" };
            _mockTaskRepo.Setup(repo => repo.GetDBResponseList(query))
                .Returns(new List<DBResponseRow> { new DBResponseRow() });

            var result = taskService.GetTask(query);

            var TaskInformation = Assert.IsType<List<TaskInformation>>(result);
            Assert.Single(TaskInformation);
        }
        [Fact]
        public void GetTeamsDetails()
        {
            _mockTaskRepo.Setup(repo => repo.TeamCountResponse())
                .Returns(new List<TeamDetails> { new TeamDetails(), new TeamDetails() });

            var result = taskService.GetTeamsDetails();

            var teamdetails = Assert.IsType<List<TeamDetails>>(result);
            Assert.Equal(2, teamdetails.Count);
        }
        [Fact]
        public void GetAssigneeCount()
        {
            string assigneeName = "Shikha";
            _mockTaskRepo.Setup(repo => repo.GetAssigneeCountResponse(assigneeName))
                .Returns(new AssigneeDetails());

            var result = taskService.GetAssigneeCount(assigneeName);

            Assert.IsType<AssigneeDetails>(result);
        }
    }
}
