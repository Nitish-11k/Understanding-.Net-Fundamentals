using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }
    public DbSet<UserModel> Users { get; set; }

}