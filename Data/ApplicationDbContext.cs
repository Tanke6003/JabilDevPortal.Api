using Microsoft.EntityFrameworkCore;
using JabilDevPortal.Api.Data.Entities;

namespace JabilDevPortal.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada entidad
        public DbSet<User>            Users          { get; set; }
        public DbSet<Application>     Applications   { get; set; }
        public DbSet<Ticket>          Tickets        { get; set; }
        public DbSet<TicketComment>   TicketComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índices únicos para usuario
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relaciones de Application
            modelBuilder.Entity<Application>()
                .HasOne(a => a.OwnerUser)
                .WithMany()
                .HasForeignKey(a => a.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.SmeUser)
                .WithMany()
                .HasForeignKey(a => a.SmeUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Application)
                .WithMany()
                .HasForeignKey(t => t.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de TicketComment
            modelBuilder.Entity<TicketComment>()
                .HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketComment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
