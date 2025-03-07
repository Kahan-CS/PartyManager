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

            // Initial Seed data
            modelBuilder.Entity<Party>().HasData(
                new Party
                {
                    PartyId = 1,
                    Description = "New Year's Eve Blast!!",
                    EventDate = new DateTime(2025, 12, 31, 0, 0, 0),
                    Location = "Time Square, NY"
                },
                new Party
                {
                    PartyId = 2,
                    Description = "Drinks at Moe's Bar",
                    EventDate = new DateTime(2025, 10, 30, 16, 43, 12),
                    Location = "Moe's Bar, Springfield"
                },
                new Party
                {
                    PartyId = 3,
                    Description = "Thanksgiving Gathering",
                    EventDate = new DateTime(2025, 10, 20, 16, 43, 12),
                    Location = "Springfield"
                }
            );

            modelBuilder.Entity<Invitation>().HasData(
                new Invitation
                {
                    InvitationId = 1,
                    GuestName = "Bob Jones",
                    GuestEmail = "pmadziak@conestogac.on.ca",
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = 1
                },
                new Invitation
                {
                    InvitationId = 2,
                    GuestName = "Sally Smith",
                    GuestEmail = "peter.madziak@gmail.com",
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = 1
                },
                new Invitation
                {
                    InvitationId = 3,
                    GuestName = "Bob Jones",
                    GuestEmail = "pmadziak@conestogac.on.ca",
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = 2
                },
                new Invitation
                {
                    InvitationId = 4,
                    GuestName = "Sally Smith",
                    GuestEmail = "peter.madziak@gmail.com",
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = 2
                },
                new Invitation
                {
                    InvitationId = 5,
                    GuestName = "Bob Jones",
                    GuestEmail = "pmadziak@conestogac.on.ca",
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = 3
                },
                new Invitation
                {
                    InvitationId = 6,
                    GuestName = "Sally Smith",
                    GuestEmail = "peter.madziak@gmail.com",
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = 3
                }
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}
