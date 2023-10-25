using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;


namespace FamilyTreeAPI
{
	public class FamilyTreeContext : DbContext
	{
        public FamilyTreeContext()
            : base()
        {
        }
        public FamilyTreeContext(DbContextOptions<FamilyTreeContext> options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Filename=../../../FamilyTree.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>().ToTable("People");
			modelBuilder.Entity<Marriage>().ToTable("Marriages");
		}
		public DbSet<Person> People { get; set; }
		public DbSet<Marriage> Marriages { get; set; }
	}
}
