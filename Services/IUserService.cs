using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Services
{
  public interface IUserService
  {
    List<UserModel> GetAllUsers();
    UserModel? GetUserById(int id);
    UserModel CreateUser(UserModel user);
  }
}