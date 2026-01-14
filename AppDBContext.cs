using Microsoft.EntityFrameworkCore;
using TodoApi.Core.Models;
using TodoApi.Core.Entities;
public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<License> Licenses { get; set; }
    public DbSet<Activation> Activations { get; set; }

}