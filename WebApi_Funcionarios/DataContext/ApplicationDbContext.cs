using Microsoft.EntityFrameworkCore;
using WebApi_ASPNETCore.Models;

namespace WebApi_ASPNETCore.DataContext;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<FuncionarioModel> Funcionarios =>
        Set<FuncionarioModel>();
}
