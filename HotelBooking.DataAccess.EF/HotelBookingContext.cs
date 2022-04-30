using HotelBooking.DataAccess.EF.Extensions;
using HotelBooking.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DataAccess.EF
{
    public partial class HotelBookingContext : DbContext
    {
        public HotelBookingContext(DbContextOptions<HotelBookingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ReservationDate> ReservationDate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Reservation");

                entity.Property(e => e.ReservationGuid).HasColumnName("ReservationGuid");
            });

            modelBuilder.Entity<ReservationDate>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ReservationDate)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_ReservationDate");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ReservationDate)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_ReservationDate");

                entity.HasIndex(p => new { p.RoomId, p.Date }).IsUnique();
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hotel_Room");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserGuid)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Seed();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
