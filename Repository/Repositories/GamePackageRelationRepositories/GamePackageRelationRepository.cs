using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageRelationRepositories
{
    public class GamePackageRelationRepository : GenericRepository<GamePackageRelation>, IGamePackageRelationRepository
    {
        public GamePackageRelationRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }
    }
}
