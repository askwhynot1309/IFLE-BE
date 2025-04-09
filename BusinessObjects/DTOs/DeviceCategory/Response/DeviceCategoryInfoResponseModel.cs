using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.DeviceCategory.Response
{
    public class DeviceCategoryInfoResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public float MinDetectionRange { get; set; }

        public float MaxDetectionRange { get; set; }

        public float HFov { get; set; }

        public float VFov { get; set; }

        public string? DeviceInfoUrl { get; set; }
    }
}
