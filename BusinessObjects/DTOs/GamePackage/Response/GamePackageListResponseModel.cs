using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.GamePackage.Response
{
    public class GamePackageListResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Duration { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = null!;

        public List<GameInfo> GameList { get; set; } = new List<GameInfo>();

    }
}
