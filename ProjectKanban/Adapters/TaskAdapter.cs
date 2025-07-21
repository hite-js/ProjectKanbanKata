using ProjectKanban.Controllers;
using ProjectKanban.Helpers;
using ProjectKanban.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ProjectKanban.Adapters
{
    public static class TaskAdapter
    {
        public static TaskModel ToModel(this TaskRecord taskRecord, List<TaskAssignedRecord> assignedRecords, List<UserRecord> userRecords) => taskRecord == null 
            ? null 
            : new TaskModel
            {
                Id = taskRecord.Id,
                Description = taskRecord.Description,
                Status = taskRecord.Status?.ToString(),
                EstimatedDevDays = taskRecord.EstimatedDevDays,
                AssignedUsers = assignedRecords.ConvertAll(assignedRecord => ToModel(assignedRecord, userRecords))
            };

        private static TaskAssignedUserModel ToModel(this TaskAssignedRecord assignedRecord, List<UserRecord> userRecords)
        {
            var user = userRecords.First(x => x.Id == assignedRecord.UserId);

            return new TaskAssignedUserModel
            {
                Initials = UsernameHelper.GetInitials(user.Username),
                Username = user.Username
            };
        }
    }
}
