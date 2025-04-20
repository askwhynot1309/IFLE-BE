using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.PlayHistory
{
    public class PlayHistoryResponse
    {
        public string Id { get; set; } = null!;

        public string GameId { get; set; } = null!;

        public string FloorId { get; set; } = null!;

        public DateTime StartAt { get; set; }

        public string UserId { get; set; } = null!;

        public int Score { get; set; }
    }

    public class PlayHistoryFloorResponse
    {
        public string Id { get; set; } = null!;

        public string GameId { get; set; } = null!;

        public string FloorId { get; set; } = null!;

        public DateTime StartAt { get; set; }

        public string UserId { get; set; } = null!;

        public int Score { get; set; }

        public GameHistory? GameHistory { get; set; }

        public UserHistory? UserHistory { get; set; }

    }

    public class GameHistory
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int PlayCount { get; set; }
    }

    public class UserHistory
    {
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
