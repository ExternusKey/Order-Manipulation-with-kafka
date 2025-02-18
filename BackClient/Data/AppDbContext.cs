using DbClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace DbClientService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<OrderConfirmation> ConfirmedOrders { get; set; }
}