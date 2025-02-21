using ServicesManipulation.Models;
using Microsoft.EntityFrameworkCore;

namespace ServicesManipulation.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OrderRequest> OrderRequests { get; set; }
    public DbSet<OrderConfirmation> ConfirmedOrders { get; set; }
}