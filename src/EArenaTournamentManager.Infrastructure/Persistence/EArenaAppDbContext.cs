using Microsoft.EntityFrameworkCore;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Common;

namespace EArenaTournamentManager.Infrastructure.Persistence
{
    public class EArenaAppDbContext : DbContext
    {
        public EArenaAppDbContext(DbContextOptions<EArenaAppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }       
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<OrganizationStaff> OrganizationStaff { get; set; }
        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region BaseEntity

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .Property(nameof(BaseEntity.CreatedBy))
                                .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                                .Property(nameof(BaseEntity.CreatedOn))
                                .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                                .Property(nameof(BaseEntity.UpdatedBy))
                                .IsRequired(false);

                    modelBuilder.Entity(entityType.ClrType)
                                .Property(nameof(BaseEntity.UpdatedOn))
                                .IsRequired(false);

                    modelBuilder.Entity(entityType.ClrType)
                                .Property(nameof(BaseEntity.IsActive))
                                .HasDefaultValue(true);
                }
            }

            #endregion

            #region User

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();                
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.AvatarImageUrl).IsRequired(false);

                entity.Property(u => u.Role)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasMany(u => u.TeamMemberships)
                      .WithOne(tm => tm.User)
                      .HasForeignKey(tm => tm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.OrganizationStaffMemberships)
                      .WithOne(os => os.User)
                      .HasForeignKey(os => os.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.CaptainedTeams)
                      .WithOne(t => t.Captain)
                      .HasForeignKey(t => t.CaptainId)
                      .OnDelete(DeleteBehavior.Restrict);                
            });

            #endregion

            #region Game

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name).IsRequired();
                entity.Property(g => g.Description).IsRequired(false);
                entity.Property(g => g.ImageUrl).IsRequired(false);

                entity.Property(g => g.Platform)
                       .HasConversion<string>()
                       .IsRequired();

                entity.HasIndex(g => g.Name).IsUnique();

                entity.HasMany(g => g.Tournaments)
                      .WithOne(t => t.Game)
                      .HasForeignKey(t => t.GameId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Team

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name).IsRequired();
                entity.Property(t => t.Description).IsRequired(false);
                entity.Property(t => t.LogoImageUrl).IsRequired(false);

                entity.HasIndex(t => t.Name).IsUnique();

                entity.HasOne(t => t.Captain)
                      .WithMany(u => u.CaptainedTeams)
                      .HasForeignKey(t => t.CaptainId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.TeamMembers)
                      .WithOne(tm => tm.Team)
                      .HasForeignKey(tm => tm.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TournamentEntries)
                      .WithOne(tp => tp.Team)
                      .HasForeignKey(tp => tp.TeamId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Organization

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.Name).IsRequired();
                entity.Property(o => o.Description).IsRequired(false);
                entity.Property(o => o.LogoImageUrl).IsRequired(false);
                entity.Property(o => o.HeaderImageUrl).IsRequired(false);

                entity.Property(o => o.Type)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasIndex(o => o.Name).IsUnique();

                entity.HasMany(o => o.StaffMembers)
                      .WithOne(os => os.Organization)
                      .HasForeignKey(os => os.OrganizationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Tournaments)
                      .WithOne(t => t.Organization)
                      .HasForeignKey(t => t.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Tournament

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name).IsRequired();
                        entity.Property(t => t.LogoImageUrl).IsRequired(false);
                entity.Property(t => t.Format).IsRequired();
                entity.Property(t => t.Map).IsRequired(false);
                entity.Property(t => t.Region).IsRequired();
                entity.Property(t => t.StartDate).IsRequired();
                entity.Property(t => t.EndDate).IsRequired();
                entity.Property(t => t.DateTime).IsRequired();
                entity.Property(t => t.Rules).IsRequired(false);
                entity.Property(t => t.Prizes).IsRequired(false);

                entity.Property(t => t.Status)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasOne(t => t.Game)
                      .WithMany(g => g.Tournaments)
                      .HasForeignKey(t => t.GameId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Organization)
                      .WithMany(g => g.Tournaments)
                      .HasForeignKey(t => t.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.Participants)
                      .WithOne(tp => tp.Tournament)
                      .HasForeignKey(tp => tp.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);                
            });

            #endregion

            #region TeamMember

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(tm => tm.Id);

                entity.Property(tm => tm.UserId).IsRequired();
                entity.Property(tm => tm.TeamId).IsRequired();

                entity.Property(tm => tm.Role).IsRequired(false);
                entity.Property(tm => tm.JoinedOn).IsRequired();

                entity.HasIndex(tm => new { tm.UserId, tm.TeamId }).IsUnique();

                entity.HasOne(tm => tm.User)
                      .WithMany(u => u.TeamMemberships)
                      .HasForeignKey(tm => tm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(tm => tm.Team)
                      .WithMany(t => t.TeamMembers)
                      .HasForeignKey(tm => tm.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region OrganizationStaff

            modelBuilder.Entity<OrganizationStaff>(entity =>
            {
                entity.HasKey(os => os.Id);

                entity.Property(os => os.OrganizationId).IsRequired();
                entity.Property(os => os.UserId).IsRequired();

                entity.Property(os => os.JoinedOn).IsRequired();

                entity.Property(os => os.Role)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasIndex(os => new { os.OrganizationId, os.UserId }).IsUnique();

                entity.HasOne(os => os.Organization)
                      .WithMany(o => o.StaffMembers)
                      .HasForeignKey(os => os.OrganizationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(os => os.User)
                      .WithMany(u => u.OrganizationStaffMemberships)
                      .HasForeignKey(os => os.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region TournamentParticipant

            modelBuilder.Entity<TournamentParticipant>(entity =>
            {
                entity.HasKey(tp => tp.Id);

                entity.Property(tp => tp.TournamentId).IsRequired();
                entity.Property(tp => tp.TeamId).IsRequired();

                entity.Property(tp => tp.JoinedOn).IsRequired();

                entity.HasIndex(tp => new { tp.TournamentId, tp.TeamId }).IsUnique();

                entity.HasOne(tp => tp.Tournament)
                      .WithMany(t => t.Participants)
                      .HasForeignKey(tp => tp.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tp => tp.Team)
                      .WithMany(t => t.TournamentEntries)
                      .HasForeignKey(tp => tp.TeamId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion
        }
    }
}
