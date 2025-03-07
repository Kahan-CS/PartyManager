using Microsoft.EntityFrameworkCore;
using PartyManager.Entities;

namespace PartyManager.Data
{
    public class PartyDbContext : DbContext
    {
        public PartyDbContext(DbContextOptions<PartyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Party>()
                .HasMany(p => p.Invitations)
                .WithOne(i => i.Party)
                .HasForeignKey(i => i.PartyId);

            // Map the InvitationStatus enum to its string representation in the database.
            modelBuilder.Entity<Invitation>()
                .Property(i => i.Status)
                .HasConversion(
                    v => v.ToString(),  // Convert enum to string when saving
                    v => (InvitationStatus)Enum.Parse(typeof(InvitationStatus), v) // String back to enum for interpretation
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
