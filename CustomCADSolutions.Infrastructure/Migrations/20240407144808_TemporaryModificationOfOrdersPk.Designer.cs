﻿// <auto-generated />
using System;
using CustomCADSolutions.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    [DbContext(typeof(CadContext))]
    [Migration("20240407144808_TemporaryModificationOfOrdersPk")]
    partial class TemporaryModificationOfOrdersPk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.Cad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Identification of 3D Model");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("B")
                        .HasColumnType("int")
                        .HasComment("rgB value of 3D Model");

                    b.Property<byte[]>("Bytes")
                        .HasColumnType("varbinary(max)")
                        .HasComment("Bytes of 3D Model");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasComment("Category of 3D Model");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2")
                        .HasComment("CreationDate of 3D Model");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Identification of the creator of the 3D Model");

                    b.Property<int>("G")
                        .HasColumnType("int")
                        .HasComment("rGb value of 3D Model");

                    b.Property<bool>("IsValidated")
                        .HasColumnType("bit")
                        .HasComment("Is 3D Model validated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)")
                        .HasComment("Name of 3D Model");

                    b.Property<int>("R")
                        .HasColumnType("int")
                        .HasComment("Rgb value of 3D Model");

                    b.Property<string>("SpinAxis")
                        .HasColumnType("nvarchar(1)")
                        .HasComment("Spin axis of 3D Model");

                    b.Property<int>("X")
                        .HasMaxLength(1000)
                        .HasColumnType("int")
                        .HasComment("X coordinate of 3D Model");

                    b.Property<int>("Y")
                        .HasMaxLength(1000)
                        .HasColumnType("int")
                        .HasComment("Y coordinate of 3D Model");

                    b.Property<int>("Z")
                        .HasMaxLength(1000)
                        .HasColumnType("int")
                        .HasComment("Z coordinate of 3D Model");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Cads");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            B = 255,
                            CategoryId = 5,
                            G = 255,
                            IsValidated = false,
                            Name = "Chair",
                            R = 255,
                            SpinAxis = "y",
                            X = 750,
                            Y = 300,
                            Z = 0
                        });
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Animals"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Characters"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Fashion"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Furniture"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Nature"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Science"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Sports"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Toys"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Vehicles"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Others"
                        });
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.Order", b =>
                {
                    b.Property<int>("CadId")
                        .HasColumnType("int")
                        .HasComment("Identification of 3D model");

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Identification of User");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)")
                        .HasComment("Description of Order");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date of Order");

                    b.Property<bool>("ShouldShow")
                        .HasColumnType("bit")
                        .HasComment("Should Order Be Visible After Completion");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("Status of Order");

                    b.HasKey("CadId");

                    b.HasIndex("BuyerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.Cad", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.Order", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppUser", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.Cad", "Cad")
                        .WithMany("Orders")
                        .HasForeignKey("CadId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("Cad");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CustomCADSolutions.Infrastructure.Data.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CustomCADSolutions.Infrastructure.Data.Models.Cad", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
