using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.GamePackage.Response
{
    public class GamePackageDetailsResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Duration { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = null!;

        public List<GameInfo> GameList { get; set; } = new List<GameInfo>();
    }

    public class GameInfo()
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? VideoUrl { get; set; }

        public int PlayCount { get; set; }

        public string DownloadUrl { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Status { get; set; } = null!;
    }
}
