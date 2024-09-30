using Microsoft.EntityFrameworkCore;
using MomoProducts.Server.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<AuthorizationData> AuthorizationData { get; set; }
}


    


