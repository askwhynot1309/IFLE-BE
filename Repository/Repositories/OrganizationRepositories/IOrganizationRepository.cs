using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.OrganizationRepositories
{
    public interface IOrganizationRepository : IGenericRepository<Organization>
    {
        Task<List<Organization>> GetAllOrganizations();

        Task<Organization> GetOrganizationById(string id);

        Task<bool> IsOrganizationNameExist(string name);
    }
}
