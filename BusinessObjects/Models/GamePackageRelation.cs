using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class GamePackageRelation
    {
        public string Id { get; set; } = null!;

        public string GameId { get; set; } = null!;

        public string GamePackageId { get; set; } = null!;

        public virtual GamePackage GamePackage { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
    }
}
