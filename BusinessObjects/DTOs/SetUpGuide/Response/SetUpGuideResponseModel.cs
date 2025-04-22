using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.SetUpGuide.Response
{
    public class SetUpGuideResponseModel
    {
        public double DistanceToFloorFromCam { get; set; }

        public double TotalLengthNeeded { get; set; }

        public double PlayFloorWidth { get; set; }

        public double PlayFloorLength { get; set; }

        public string? Notice { get; set; }
    }
}
