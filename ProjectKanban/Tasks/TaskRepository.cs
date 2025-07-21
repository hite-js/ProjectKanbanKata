using System.Collections.Generic;
using System.Linq;
using Dapper;
using ProjectKanban.Data;
using ProjectKanban.Users;

namespace ProjectKanban.Tasks
{
    public sealed class TaskRepository
    {
        private readonly IDatabase _database;

        public TaskRepository(IDatabase database)
        {
            _database = database;
        }

        public TaskRecord GetById(int id)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskRecord>("SELECT * from task WHERE id = @TaskId;", new { TaskId = id }).FirstOrDefault();
                return taskRecords;
            }
        }

        public TaskRecord Create(TaskRecord taskRecord)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                taskRecord.Id = connection.Insert("insert into task(status, description, estimated_dev_days, client_id) VALUES (@Status, @Description, @EstimatedDevDays, @ClientId);", taskRecord);
                transaction.Commit();
            }

            return taskRecord;
        }

        public List<TaskRecord> GetAll(Session session)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();

                string query = $"""
                SELECT t.*
                FROM task t
                INNER JOIN user u ON t.client_id = u.client_id
                WHERE u.id = @UserId
                ORDER BY 
                    CASE t.status
                        WHEN '{TaskStatus.DONE}' THEN 1
                        WHEN '{TaskStatus.IN_SIGNOFF}' THEN 2
                        WHEN '{TaskStatus.IN_PROGRESS}' THEN 3
                        WHEN '{TaskStatus.BACKLOG}' THEN 4
                        ELSE 5
                    END;
                """;
                var taskRecords = connection.Query<TaskRecord>(query, new { UserId = session.UserId }).ToList();
                return taskRecords;
            }
        }

        public List<TaskAssignedRecord> GetAssignedFor(int taskId)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskAssignedRecord>("SELECT * from task_assigned where task_id = @TaskId;", new {TaskId = taskId}).ToList();
                return taskRecords;
            }
        }
        
        public void CreateAssigned(TaskAssignedRecord record)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                connection.Execute("INSERT INTO task_assigned (task_id, user_id) VALUES (@TaskId, @UserId);", record);
                transaction.Commit();
            }
        }
    }

    public class TaskRecord
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int EstimatedDevDays { get; set; }
    }
    
    public class UserRecord
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class ClientRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class TaskAssignedRecord
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }

    public struct TaskStatus
    {
        public const string BACKLOG = "Backlog";
        public const string IN_PROGRESS = "In Progress";
        public const string IN_SIGNOFF = "In Signoff";
        public const string DONE = "Done";
    }
}