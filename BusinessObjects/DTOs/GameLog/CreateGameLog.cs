using System;

namespace BusinessObjects.DTOs.GameLog
{
    public class CreateGameLog
    {
        public string Description { get; set; } = null!;
        public string StaffId { get; set; } = null!;
        public string GameId { get; set; } = null!;
    }
}
