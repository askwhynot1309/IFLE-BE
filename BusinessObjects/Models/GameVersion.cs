using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class GameVersion
    {
        public string Id { get; set; } = null!;

        public string Version { get; set; } = null!;

        public DateTime ReleaseDate {  get; set; }

        public string? Description { get; set; }

        public string GameId { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
    }
}
