using ServicesManipulation.Models;
using Microsoft.EntityFrameworkCore;

namespace ServicesManipulation.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<OrderRequest> OrderRequests { get; set; }
}