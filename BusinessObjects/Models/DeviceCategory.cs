using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class DeviceCategory
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public float MinDetectionRange { get; set; }

        public float MaxDetectionRange { get; set; }

        public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    }
}
