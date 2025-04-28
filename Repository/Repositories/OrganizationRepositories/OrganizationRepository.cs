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

        public async Task<bool> IsOrganizationNameExist(string name)
        {
            var organization = await GetSingle(o => o.Name.ToLower().Equals(name.ToLower()));
            return organization != null;
        }
    }
}
