using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ProjectKanban.Adapters;
using ProjectKanban.Controllers;
using ProjectKanban.Helpers;
using ProjectKanban.Users;

namespace ProjectKanban.Tasks
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;

        public TaskService(TaskRepository taskRepository, UserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public TaskModel GetById(Session session, int id)
        {
            var taskRecord = _taskRepository.GetById(id);

            if (taskRecord == null)
            {
                return null;
            }

            List<TaskAssignedRecord> assigned = _taskRepository.GetAssignedFor(id);

            return TaskAdapter.ToModel(taskRecord, assigned, _userRepository.GetAll());
        }

        public GetAllTasksResponse GetAll(Session session)
        {
            var taskRecords = _taskRepository.GetAll(session);

            var response = new GetAllTasksResponse{Tasks = new List<TaskModel>()};

            return new GetAllTasksResponse { Tasks = taskRecords.ConvertAll(taskRecords => TaskAdapter.ToModel(taskRecords, _taskRepository.GetAssignedFor(taskRecords.Id), _userRepository.GetAll())) };
        }
    }
}