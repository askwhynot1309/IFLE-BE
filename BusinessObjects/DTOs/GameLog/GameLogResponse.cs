using System;

namespace BusinessObjects.DTOs.GameLog
{
    public class GameLogResponse
    {
        public string Id { get; set; } = null!;
        public DateTime UpdateTime { get; set; }
        public string Description { get; set; } = null!;
        public string StaffId { get; set; } = null!;
        public string GameId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string GameTitle { get; set; } = null!;
    }
} 