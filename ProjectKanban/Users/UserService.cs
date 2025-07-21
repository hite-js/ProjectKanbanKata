using System;
using System.Collections.Generic;
using System.Linq;
using ProjectKanban.Controllers;
using ProjectKanban.Helpers;

namespace ProjectKanban.Users
{
    public sealed class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AllUsersResponse GetAllUsers()
        {
            var userRecords = _userRepository.GetAll();
            return new AllUsersResponse { Users = userRecords.ConvertAll(UserAdapter.ToModel) };
        }

        public Session Login(LoginRequest loginRequest)
        {
            var user = _userRepository.GetAll().FirstOrDefault(x => x.Username == loginRequest.Username && x.Password == loginRequest.Password);
            if (user != null)
                return new Session
                {
                    Username = user.Username,
                    UserId = user.Id
                };
            throw new Exception("Invalid credentials");
        }
    }
}