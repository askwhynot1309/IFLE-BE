﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class RefreshToken
    {
        public string Id { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpiredAt { get; set; }

        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
