﻿// <auto-generated />
using System;
using HotelBooking.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelBooking.DataAccess.EF.Migrations
{
    [DbContext(typeof(HotelBookingContext))]
    partial class HotelBookingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelBooking.Model.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Hotel");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Cancun Hotel"
                        });
                });

            modelBuilder.Entity("HotelBooking.Model.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnName("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("ReservationGuid")
                        .HasColumnName("ReservationGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("HotelBooking.Model.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Room");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            HotelId = 1,
                            Name = "Room 101"
                        });
                });

            modelBuilder.Entity("HotelBooking.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserGuid")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HotelBooking.Model.Reservation", b =>
                {
                    b.HasOne("HotelBooking.Model.Room", "Room")
                        .WithMany("Reservation")
                        .HasForeignKey("RoomId")
                        .HasConstraintName("FK_Room_Reservation")
                        .IsRequired();

                    b.HasOne("HotelBooking.Model.User", "User")
                        .WithMany("Reservation")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_User_Reservation")
                        .IsRequired();
                });

            modelBuilder.Entity("HotelBooking.Model.Room", b =>
                {
                    b.HasOne("HotelBooking.Model.Hotel", "Hotel")
                        .WithMany("Room")
                        .HasForeignKey("HotelId")
                        .HasConstraintName("FK_Hotel_Room")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
