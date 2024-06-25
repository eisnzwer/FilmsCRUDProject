using FilmsCRUDProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmsCRUDProject.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<Film> Films { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.HasMany(u => u.Films)
			.WithMany(f => f.Users);
	}
}