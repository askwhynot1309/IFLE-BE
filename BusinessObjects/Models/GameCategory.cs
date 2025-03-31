using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class GameCategory
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<GameCategoryRelation> GameCategoryRelations { get; set; } = new List<GameCategoryRelation>();
    }
}
