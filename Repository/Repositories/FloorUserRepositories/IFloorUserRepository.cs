using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.FloorUserRepositories
{
    public interface IFloorUserRepository : IGenericRepository<FloorUser>
    {
        Task<List<string>> GetListUserIdByFloorId(string floorId);
    }
}
