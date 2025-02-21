using DbClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace DbClientService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OrderConfirmation> ConfirmedOrders { get; set; }
    public DbSet<OrderRequest> OrderRequests { get; set; }
}