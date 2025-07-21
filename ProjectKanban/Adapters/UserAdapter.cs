using ProjectKanban.Controllers;
using ProjectKanban.Helpers;
using ProjectKanban.Tasks;

namespace ProjectKanban.Adapters
{
    public static class UserAdapter
    {
        public static UserModel ToModel(this UserRecord userRecord)
            => userRecord == null
            ? null
            : new UserModel
            {
                Id = userRecord.Id,
                Username = userRecord.Username,
                Initials = UsernameHelper.GetInitials(userRecord.Username)
            };
    }
}
