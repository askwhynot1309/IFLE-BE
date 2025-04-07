using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.OrganizationRepositories
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<Organization>> GetAllOrganizations()
        {
            return (await Get()).ToList();
        }

        public async Task<Organization> GetOrganizationById(string id)
        {
            return await GetSingle(o => o.Id.Equals(id));
        }
    }
}
