using AutoMapper;
using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.Organization.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.OrganizationRepositories;
using Repository.Repositories.OrganizationUserRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.AuthenticationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task CreateOrganization(OrganizationCreateRequestModel model, string userId)
        {
            var newOrganization = _mapper.Map<Organization>(model);

            newOrganization.Id = Guid.NewGuid().ToString();
            newOrganization.CreatedAt = DateTime.Now;
            newOrganization.UserLimit = 2;

            await _organizationRepository.Insert(newOrganization);

            var newOrganizationUser = new OrganizationUser
            {
                Id = Guid.NewGuid().ToString(),
                OrganizationId = newOrganization.Id,
                JoinedAt = DateTime.Now,
                UserId = userId,
                Privilege = PrivilegeEnums.Owner.ToString(),
            };

            await _organizationUserRepository.Insert(newOrganizationUser);
        }

        public async Task<List<OrganizationInfoResponseModel>> ViewAllOrganizations()
        {
            var organizations = await _organizationRepository.GetAllOrganizations();
            var result = _mapper.Map<List<OrganizationInfoResponseModel>>(organizations);
            return result;
        }

        public async Task<List<OrganizationUserReponseModel>> GetMembersOfOrganization(string organizationId)
        {
            var listUserId = await _organizationUserRepository.GetUserOfOrganization(organizationId);

            var users = await _userRepository.GetUsersByIdList(listUserId);

            var result = new List<OrganizationUserReponseModel>();

            foreach (var user in users)
            {
                var newOrganizationUserReponseModel = new OrganizationUserReponseModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    AvatarUrl = user.AvatarUrl,
                    Email = user.Email,
                    Privilege = user.OrganizationUsers.First(u => u.OrganizationId.Equals(organizationId)).Privilege
                };
                result.Add(newOrganizationUserReponseModel);
            }
            return result;
        }

        public async Task<List<OrganizationInfoResponseModel>> GetOwnOrganization(string userId)
        {
            var organizationList = await _organizationUserRepository.GetOrganizationOfUser(userId);

            var result = _mapper.Map<List<OrganizationInfoResponseModel>>(organizationList);
            return result;
        }
    }
}
