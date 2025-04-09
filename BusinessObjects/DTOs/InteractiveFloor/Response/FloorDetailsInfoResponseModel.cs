using BusinessObjects.DTOs.DeviceCategory.Response;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.InteractiveFloor.Response
{
    public class FloorDetailsInfoResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Length { get; set; }

        public bool IsPublic { get; set; }

        public string Status { get; set; } = null!;

        public DeviceInfo? DeviceInfo { get; set; }
    }

    public class DeviceInfo
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Uri { get; set; } = null!;

        public DeviceCategoryInfoResponseModel? DeviceCategory { get; set; }
    }
}
