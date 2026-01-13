using TodoApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Services
{
  public class UserService 
  {
    private readonly AppDBContext _context;

    public UserService(AppDBContext context)
    {
      this._context = context;
    }

    public async Task<UserModel> CreateUserAsync(UserModel user)
    {
      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task<List<UserModel>> GetAllUserAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public async Task<UserModel?> GetUserByIdAsync(int id)
    {
      return await _context.Users.FindAsync(id);
    }

    public async Task<UserModel?> UpdateUserAsync(UserModel user)
    {
      var existingUser = await _context.Users.FindAsync(user.Id);
      if (existingUser == null)
      {
        return null;
      }

      existingUser.Name = user.Name;
      existingUser.Email = user.Email;
      existingUser.PasswordHash = user.PasswordHash;

      await _context.SaveChangesAsync();
      return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null)
      {
        return false;
      }

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}