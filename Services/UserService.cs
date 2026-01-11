using TodoApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Services
{
  public class UserService : IUserService
  {
    private static List<UserModel> _users = new List<UserModel>()
    {
      new UserModel { Id = 1, Name = "Admin", Email = "abc@gmail.com" },
      new UserModel { Id = 2, Name = "User1", Email = "user1@gmail.com"}
    };

    public List<UserModel> GetAllUsers()
    {
      return _users;
    }
    public UserModel GetUserById(int id)
    {
      return _users.Find(user => user.Id == id);
    }

    public UserModel CreateUser(UserModel user)
    {
      user.Id = _users.Count + 1; // Simple ID assignment
      _users.Add(user);
      return user;
    }
  }
}