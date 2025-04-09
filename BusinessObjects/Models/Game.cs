using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Game
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? VideoUrl { get; set; }

        public int PlayCount { get; set; }

        public string DownloadUrl { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<GamePackageRelation> GamePackageRelations { get; set; } = new List<GamePackageRelation>();

        public virtual ICollection<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();

        public virtual ICollection<GameCategoryRelation> GameCategoryRelations { get; set; } = new List<GameCategoryRelation>();

        public virtual ICollection<GameVersion> GameVersions { get; set; } = new List<GameVersion>();
    }
}
