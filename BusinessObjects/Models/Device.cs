using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Device
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string SerialNumber { get; set; } = null!;

        public string DeviceCategoryId { get; set; } = null!;

        public virtual DeviceCategory DeviceCategory { get; set; } = null!;

        public virtual ICollection<InteractiveFloor> InteractiveFloors { get; set; } = new List<InteractiveFloor>();

    }
}
