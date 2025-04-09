using AutoMapper;
using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.Organization.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Repository.Enums;
using Repository.Repositories.OrganizationRepositories;
using Repository.Repositories.OrganizationUserRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.AuthenticationServices;
using Service.Services.EmailServices;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
        private readonly IEmailService _emailService;

        public OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task CreateOrganization(OrganizationCreateUpdateRequestModel model, string userId)
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

            var users = await _userRepository.GetCustomerListById(listUserId);

            var result = new List<OrganizationUserReponseModel>();

            foreach (var user in users)
            {
                var newOrganizationUserReponseModel = new OrganizationUserReponseModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    AvatarUrl = user.AvatarUrl,
                    Email = user.Email,
                    Privilege = user.OrganizationUsers.First(u => u.OrganizationId.Equals(organizationId)).Privilege,
                    Status = user.Status
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

        public async Task UpdateOrganization(string id, OrganizationCreateUpdateRequestModel model)
        {
            var organization = await _organizationRepository.GetOrganizationById(id);
            if (organization == null)
            {
                throw new CustomException("Không tồn tại tổ chức nào.");
            }
            _mapper.Map(model, organization);
            await _organizationRepository.Update(organization);
        }

        public async Task AddUserToOrganization(List<string> emailList, string organizationId, string currentId)
        {
            var currentUser = await _organizationUserRepository.GetOrganizationUserByUserId(currentId, organizationId);

            if (currentUser == null)
            {
                throw new CustomException("Không tìm thấy người dùng này.");
            }

            if (currentUser.Privilege.Equals(PrivilegeEnums.Member.ToString()))
            {
                throw new CustomException("Không có quyền thực hiện hành động này.", StatusCodes.Status403Forbidden);
            }

            var userIdList = await _userRepository.GetUserIdListByEmail(emailList);
            if (userIdList.Count() == 0)
            {
                throw new CustomException("Không có người dùng nào được chọn.");
            }

            var organization = await _organizationRepository.GetOrganizationById(organizationId);

            var organizationUsers = await _organizationUserRepository.GetOrganizationUserByOrganizationId(organizationId);

            var currentAmount = organizationUsers.Count();
            if (userIdList.Count() + currentAmount > organization.UserLimit)
            {
                throw new CustomException("Số lượng người vượt quá giới hạn cho phép của tổ chức.");
            }

            var existedUser = organizationUsers.Select(o => o.UserId).ToList();
            var list = userIdList.Intersect(existedUser);
            if (list.Count() > 0)
            {
                throw new CustomException("Người dùng đã được thêm vào tổ chức trước đó.");
            }

            var organizationUserList = new List<OrganizationUser>();
            var userList = await _userRepository.GetCustomerListById(userIdList);
            var models = new List<AddMemberEmailModel>();

            foreach (var user in userList)
            {
                models.Add(new AddMemberEmailModel
                {
                    Email = user.Email,
                    HtmlBody = HTMLEmailTemplate.SendingOrganizationInvitationEmail(user.FullName, user.Email, DateTime.Now.ToString("dd/MM/yyyy"), organization.Name)
                });
            }

            bool sendEmailSuccess = await _emailService.SendEmailList(models, "Lời mời tham gia tổ chức");
            if (!sendEmailSuccess)
            {
                throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
            }

            foreach (var userId in userIdList)
            {
                organizationUserList.Add(new OrganizationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = organizationId,
                    UserId = userId,
                    JoinedAt = DateTime.Now,
                    Privilege = PrivilegeEnums.Member.ToString(),
                });
            }

            await _organizationUserRepository.InsertRange(organizationUserList);
        }

        public async Task RemoveMemberFromOrganization(List<string> userIdList, string organizationId, string currentId)
        {
            var currentUser = await _organizationUserRepository.GetOrganizationUserByUserId(currentId, organizationId);

            if (currentUser == null)
            {
                throw new CustomException("Không tìm thấy người dùng này.");
            }

            if (!currentUser.Privilege.Equals(PrivilegeEnums.Owner.ToString()))
            {
                throw new CustomException("Không có quyền thực hiện hành động này.", StatusCodes.Status403Forbidden);
            }

            if (userIdList.Contains(currentId))
            {
                throw new CustomException("Không được phép xóa người dùng này.");
            }

            var removeList = await _organizationUserRepository.GetOrganizationUsersByUserIdList(userIdList, organizationId);
            if (removeList.Count() > 0)
            {
                await _organizationUserRepository.DeleteRange(removeList);
            }
            else
            {
                throw new CustomException("Không tồn tại người dùng để xóa khỏi tổ chức.");
            }
        }

        public async Task GrantPrivilege(List<string> userIdList, string organizationId)
        {
            var grantList = await _organizationUserRepository.GetOrganizationUsersByUserIdList(userIdList, organizationId);
            if (grantList.Count() < 0)
            {
                throw new CustomException("Không có người dùng nào được chọn.");
            }
            foreach (var member in grantList)
            {
                member.Privilege = PrivilegeEnums.CoOwner.ToString();
            }
            await _organizationUserRepository.UpdateRange(grantList);
        }

        public async Task RemovePrivilege(List<string> userIdList, string organizationId)
        {
            var removeList = await _organizationUserRepository.GetOrganizationUsersByUserIdList(userIdList, organizationId);
            if (removeList.Count() < 0)
            {
                throw new CustomException("Không có người dùng nào được chọn.");
            }
            foreach (var member in removeList)
            {
                member.Privilege = PrivilegeEnums.Member.ToString();
            }
            await _organizationUserRepository.UpdateRange(removeList);
        }

        public async Task<OrganizationInfoResponseModel> GetDetailsInfoOfOrganization(string organizationId)
        {
            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            return _mapper.Map<OrganizationInfoResponseModel>(organization);
        }
    }
}
