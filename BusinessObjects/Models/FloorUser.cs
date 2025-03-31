using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class FloorUser
    {
        public string Id { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string FloorId { get; set; } = null!;

        public virtual User User { get; set; } = null!;

        public virtual InteractiveFloor InteractiveFloor { get; set; } = null!;
    }
}
