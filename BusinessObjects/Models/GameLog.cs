using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class GameLog
    {
        public string Id { get; set; } = null!;

        public DateTime UpdateTime { get; set; }

        public string Description { get; set; } = null!;

        public string UpdateStatus { get; set; } = null!;

        public string StaffId { get; set; } = null!;

        public string GameId { get; set; } = null!;

        public virtual User Staff { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
    }
}
