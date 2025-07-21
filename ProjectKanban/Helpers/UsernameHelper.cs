using System;
using System.Linq;

namespace ProjectKanban.Helpers
{
    public static class UsernameHelper
    {
        public static string GetInitials(string username)
        {
            if (string.IsNullOrEmpty(username))
                return string.Empty;

            string[] names = username.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(names.Select(name => char.ToUpper(name[0])));
        }
    }
}
