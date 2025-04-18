using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class User
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string Salt { get; set; } = null!;

        public string RoleId { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = null!;

        public bool IsVerified { get; set; }

        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<OTP> OTP { get; set; } = new List<OTP>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public virtual ICollection<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();

        public virtual ICollection<PrivateFloorUser> PrivateFloorUsers { get; set; } = new List<PrivateFloorUser>();

        public virtual ICollection<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();

        public virtual ICollection<GamePackageOrder> GamePackageOrders { get; set; } = new List<GamePackageOrder>();

        public virtual ICollection<UserPackageOrder> UserPackageOrders { get; set; } = new List<UserPackageOrder>();
    }
}
