using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class PlayHistory
    {
        public string Id { get; set; } = null!;

        public string GameId { get; set; } = null!;

        public string FloorId { get; set; } = null!;

        public DateTime StartAt { get; set; }

        public string UserId { get; set; } = null!;

        public int Score { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual InteractiveFloor InteractiveFloor { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
    }
}
