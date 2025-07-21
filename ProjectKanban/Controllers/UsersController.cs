using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProjectKanban.Users;

namespace ProjectKanban.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private UserService _userService;

        public UsersController(UserRepository userRepository)
        {
            _userService = new UserService(userRepository);
        }
        
        [HttpGet("{id}")]
        public UserModel Get(int id)
        {
            return new UserModel();
        }
        
        [HttpGet("")]
        public AllUsersResponse GetAll()
        {
            return _userService.GetAllUsers();
        }

        [HttpPost("login")]
        public Session Login([FromBody] LoginRequest loginRequest)
        {
            return _userService.Login(loginRequest);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Initials { get; set; }
    }

    public class AllUsersResponse
    {
        public List<UserModel> Users { get; set; }
    }
}