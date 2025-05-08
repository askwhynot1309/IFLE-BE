using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public partial class InteractiveFloorManagementContext : DbContext
    {
        public InteractiveFloorManagementContext()
        {
        }

        public InteractiveFloorManagementContext(DbContextOptions<InteractiveFloorManagementContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<InteractiveFloor> InteractiveFloors { get; set; }

        public virtual DbSet<Device> Devices { get; set; }

        public virtual DbSet<UserPackage> UserPackages { get; set; }

        public virtual DbSet<GamePackage> GamePackages { get; set; }

        public virtual DbSet<DeviceCategory> DeviceCategories { get; set; }

        public virtual DbSet<GameCategory> GameCategories { get; set; }

        public virtual DbSet<GameVersion> GameVersions { get; set; }

        public virtual DbSet<PrivateFloorUser> PrivateFloorUsers { get; set; }

        public virtual DbSet<GameCategoryRelation> GameCategoryRelations { get; set; }

        public virtual DbSet<GamePackageOrder> GamePackageOrders { get; set; }

        public virtual DbSet<GamePackageRelation> GamePackageRelations { get; set; }

        public virtual DbSet<OrganizationUser> OrganizationUsers { get; set; }

        public virtual DbSet<PlayHistory> PlayHistories { get; set; }

        public virtual DbSet<UserPackageOrder> UserPackageOrders { get; set; }

        public virtual DbSet<GameLog> GameLogs { get; set; }

        public virtual DbSet<ActiveUser> ActiveUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Password)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Salt)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.RoleId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.IsVerified)
                    .HasColumnType("bit")
                    .IsRequired();

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = "9a91761a-afb5-49ac-b42e-bf357e944eab", Name = "Admin" },
                new Role { Id = "eade30a8-5e6e-4103-85b2-7e449de61a8b", Name = "Staff" },
                new Role { Id = "bc60ffc5-e9bb-4c9d-916a-69c673fbb184", Name = "Customer" }
                );

            modelBuilder.Entity<OrganizationUser>(entity =>
            {
                entity.ToTable("OrganizationUser");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.OrganizationId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Privilege)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.JoinedAt)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(e => e.OrganizationUsers)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Organization)
                    .WithMany(e => e.OrganizationUsers)
                    .HasForeignKey(e => e.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.UserLimit)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<UserPackageOrder>(entity =>
            {
                entity.ToTable("UserPackageOrder");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.UserPackageId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.OrganizationId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(e => e.UserPackageOrders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Organization)
                    .WithMany(e => e.UserPackageOrders)
                    .HasForeignKey(e => e.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.UserPackage)
                    .WithMany(e => e.UserPackageOrders)
                    .HasForeignKey(e => e.UserPackageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserPackage>(entity =>
            {
                entity.ToTable("UserPackage");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.UserLimit)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<PrivateFloorUser>(entity =>
            {
                entity.ToTable("PrivateFloorUser");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.FloorId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PrivateFloorUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.InteractiveFloor)
                    .WithMany(p => p.PrivateFloorUsers)
                    .HasForeignKey(d => d.FloorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InteractiveFloor>(entity =>
            {
                entity.ToTable("InteractiveFloor");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Height)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.Width)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.Length)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.IsPublic)
                    .HasColumnType("bit")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.OrganizationId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.InteractiveFloors)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OTP>(entity =>
            {
                entity.ToTable("OTP");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.User).WithMany(p => p.OTP)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Uri)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.DeviceCategoryId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.DeviceCategory)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.DeviceCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DeviceCategory>(entity =>
            {
                entity.ToTable("DeviceCategory");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.MinDetectionRange)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.MaxDetectionRange)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.HFov)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.VFov)
                    .HasColumnType("float")
                    .IsRequired();

                entity.Property(e => e.DeviceInfoUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<PlayHistory>(entity =>
            {
                entity.ToTable("PlayHistory");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.FloorId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.StartAt)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.Score)
                    .HasColumnType("int")
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.PlayHistories)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PlayHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.InteractiveFloor)
                    .WithMany(p => p.PlayHistories)
                    .HasForeignKey(d => d.FloorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.PlayCount)
                    .HasColumnType("int")
                    .IsRequired();

                entity.Property(e => e.DownloadUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<GameVersion>(entity =>
            {
                entity.ToTable("GameVersion");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameVersions)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GameCategory>(entity =>
            {
                entity.ToTable("GameCategory");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<GameCategoryRelation>(entity =>
            {
                entity.ToTable("GameCategoryRelation");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.GameCategoryId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameCategoryRelations)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.GameCategory)
                    .WithMany(p => p.GameCategoryRelations)
                    .HasForeignKey(d => d.GameCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GamePackageOrder>(entity =>
            {
                entity.ToTable("GamePackageOrder");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.IsActivated)
                    .HasColumnType("bit");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GamePackageOrders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.InteractiveFloor)
                    .WithMany(p => p.GamePackageOrders)
                    .HasForeignKey(d => d.FloorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.GamePackage)
                    .WithMany(p => p.GamePackageOrders)
                    .HasForeignKey(d => d.GamePackageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GamePackage>(entity =>
            {
                entity.ToTable("GamePackage");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<GamePackageRelation>(entity =>
            {
                entity.ToTable("GamePackageRelation");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.GamePackageId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamePackageRelations)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.GamePackage)
                    .WithMany(p => p.GamePackageRelations)
                    .HasForeignKey(d => d.GamePackageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Token)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.ExpiredAt)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GameLog>(entity =>
            {
                entity.ToTable("GameLog");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.StaffId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GameId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.UpdateStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.Staff).WithMany(p => p.GameLogs)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Game).WithMany(p => p.GameLogs)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ActiveUser>(entity =>
            {
                entity.ToTable("ActiveUser");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.LoginTime)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit")
                    .IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
