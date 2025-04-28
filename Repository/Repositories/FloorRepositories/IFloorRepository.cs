using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.FloorRepositories
{
    public interface IFloorRepository : IGenericRepository<InteractiveFloor>
    {
        Task<InteractiveFloor> GetFloorById(string floorId);

        Task<List<InteractiveFloor>> GetAllPublicFloorsOfOrganization(string organizationId);

        Task<List<InteractiveFloor>> GetAllPrivateFloorsOfOrganization(string organizationId);

        Task<bool> IsFloorNameExistInOrganization(string organizationId, string name);
    }
}
