using Microsoft.EntityFrameworkCore;

namespace FamilyTreeAPI
{
	public class FamilyTreeContext : DbContext
	{
		public FamilyTreeContext(DbContextOptions<FamilyTreeContext> options)
			: base(options)
		{
		}
		public DbSet<Person> People { get; set; }
		public DbSet<Marriage> Marriages { get; set; }
	}
}
