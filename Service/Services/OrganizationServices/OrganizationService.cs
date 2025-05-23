﻿using AutoMapper;
using BusinessObjects.DTOs.GamePackageOrder.Request;
using BusinessObjects.DTOs.InteractiveFloor.Response;
using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.Organization.Response;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.DTOs.UserPackage.Response;
using BusinessObjects.DTOs.UserPackageOrder.Request;
using BusinessObjects.DTOs.UserPackageOrder.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Net.payOS.Types;
using Repository.Enums;
using Repository.Repositories.FloorRepositories;
using Repository.Repositories.FloorUserRepositories;
using Repository.Repositories.GamePackageOrderRepositories;
using Repository.Repositories.OrganizationRepositories;
using Repository.Repositories.OrganizationUserRepositories;
using Repository.Repositories.UserPackageOrderRepositories;
using Repository.Repositories.UserPackageRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.AuthenticationServices;
using Service.Services.EmailServices;
using Service.Services.PayosServices;
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
        private readonly IFloorRepository _floorRepository;
        private readonly IPrivateFloorUserRepository _privateFloorUserRepository;
        private readonly IUserPackageRepository _userPackageRepository;
        private readonly IPayosService _payosService;
        private readonly IUserPackageOrderRepository _userPackageOrderRepository;
        private readonly IGamePackageOrderRepository _gamePackageOrderRepository;

        public OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository, IEmailService emailService, IFloorRepository floorRepository, IPrivateFloorUserRepository privateFloorUserRepository, IUserPackageRepository userPackageRepository, IUserPackageOrderRepository userPackageOrderRepository, IPayosService payosService, IGamePackageOrderRepository gamePackageOrderRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _emailService = emailService;
            _floorRepository = floorRepository;
            _privateFloorUserRepository = privateFloorUserRepository;
            _userPackageRepository = userPackageRepository;
            _userPackageOrderRepository = userPackageOrderRepository;
            _payosService = payosService;
            _gamePackageOrderRepository = gamePackageOrderRepository;
        }

        public async Task CreateOrganization(OrganizationCreateUpdateRequestModel model, string userId)
        {
            var newOrganization = _mapper.Map<Organization>(model);
            var nameCheck = await _organizationUserRepository.IsCreatedOrganizationNameExist(userId, model.Name);
            if (nameCheck)
            {
                throw new CustomException("Tên tổ chức này đã được bạn sử dụng. Vui lòng đặt tên khác.");
            }

            newOrganization.Id = Guid.NewGuid().ToString();
            newOrganization.CreatedAt = DateTime.Now;
            newOrganization.UserLimit = 2;
            newOrganization.Status = OrganizationStatusEnums.Active.ToString();

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

        public async Task<List<OrganizationAllInfoResponseModel>> ViewAllOrganizations()
        {
            var organizations = await _organizationRepository.GetAllOrganizations();
            var result = organizations.Select(o => new OrganizationAllInfoResponseModel
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                CreatedAt = o.CreatedAt,
                UserLimit = o.UserLimit,
                Status = o.Status,
                OwnerInfo = _mapper.Map<UserInfoResponeModel>(o.OrganizationUsers.FirstOrDefault(o => o.Privilege.Equals(PrivilegeEnums.Owner.ToString())).User)
            }).ToList();
            return result.OrderByDescending(r => r.CreatedAt).ToList();
        }

        public async Task<List<OrganizationUserReponseModel>> GetMembersOfOrganization(string organizationId, string currentId)
        {
            var listUserId = await _organizationUserRepository.GetUserOfOrganization(organizationId);
            var organizationIdList = (await _organizationUserRepository.GetOrganizationUserListByUserId(currentId)).Select(o => o.OrganizationId).ToList();

            if (!organizationIdList.Contains(organizationId))
            {
                throw new CustomException("Người dùng không thuộc tổ chức này.");
            }

            var users = await _userRepository.GetCustomerListByIdList(listUserId);

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
            return result.OrderBy(r => r.FullName).ToList();
        }

        public async Task<List<OrganizationInfoResponseModel>> GetOwnOrganization(string userId)
        {
            var organizationList = await _organizationUserRepository.GetOrganizationOfUser(userId);
            var result = new List<OrganizationInfoResponseModel>();
            foreach (var organization in organizationList)
            {
                result.Add(new OrganizationInfoResponseModel
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    Description = organization.Description,
                    UserLimit = organization.UserLimit,
                    CreatedAt = organization.CreatedAt,
                    Status = organization.Status,
                    Privilege = organization.OrganizationUsers.FirstOrDefault(o => o.OrganizationId.Equals(organization.Id)).Privilege
                });
            }
            return result.OrderByDescending(r => r.CreatedAt).ToList();
        }

        public async Task UpdateOrganization(string id, OrganizationCreateUpdateRequestModel model, string currentUserId)
        {
            var organization = await _organizationRepository.GetOrganizationById(id);
            if (organization == null)
            {
                throw new CustomException("Không tồn tại tổ chức này.");
            }

            var ownerId = await _organizationUserRepository.GetOwnerIdOfOrganization(organization.Id);
            if (!ownerId.Equals(currentUserId))
            {
                throw new CustomException("Không phải là chủ sở hữu tổ chức này", StatusCodes.Status403Forbidden);
            }

            if (!model.Name.ToLower().Equals(organization.Name.ToLower()))
            {
                var nameCheck = await _organizationUserRepository.IsCreatedOrganizationNameExist(currentUserId, model.Name);
                if (nameCheck)
                {
                    throw new CustomException("Tên tổ chức này đã được bạn sử dụng. Vui lòng đặt tên khác.");
                }
            }

            _mapper.Map(model, organization);
            await _organizationRepository.Update(organization);
        }

        public async Task SoftRemoveOrganization(string organizationId, string currentUserId)
        {
            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            if (organization == null)
            {
                throw new CustomException("Không tồn tại tổ chức này.");
            }

            var ownerId = await _organizationUserRepository.GetOwnerIdOfOrganization(organizationId);
            if (!ownerId.Equals(currentUserId))
            {
                throw new CustomException("Bạn không phải là chủ sở hữu tổ chức này", StatusCodes.Status403Forbidden);
            }

            var floorList = await _floorRepository.GetAllActiveFloorOfOrganization(organizationId);
            var floorIdList = floorList.Select(x => x.Id).ToList();

            var now = DateTime.Now;
            var check = await _gamePackageOrderRepository.CheckIfAnyAvailableGamePackageInFloorList(floorIdList, now);
            if (check)
            {
                throw new CustomException("Còn các gói game còn hạn hoặc chưa được kích hoạt trong tổ chức này. Không thể xóa.");
            }

            organization.Status = OrganizationStatusEnums.Inactive.ToString();
            await _organizationRepository.Update(organization);
        }

        public async Task AddUserToOrganization(List<string> emailList, string organizationId, string currentId)
        {
            var currentUser = await _organizationUserRepository.GetOrganizationUserByUserIdAndOrganizationId(currentId, organizationId);

            var addList = await _userRepository.GetUserListByEmailList(emailList);

            if (!addList.All(u => u.Role.Name.Equals(RoleEnums.Customer.ToString())))
            {
                throw new CustomException("Không thể thêm admin hoặc staff vào tổ chức.");
            }

            if (currentUser == null)
            {
                throw new CustomException("Không tìm thấy người dùng này.");
            }

            if (currentUser.Privilege.Equals(PrivilegeEnums.Member.ToString()))
            {
                throw new CustomException("Không có quyền thực hiện hành động này.", StatusCodes.Status403Forbidden);
            }

            var userIdList = await _userRepository.GetUserIdListByEmailList(emailList);
            if (userIdList.Count == 0)
            {
                throw new CustomException("Không có người dùng nào được chọn hoặc người dùng được chọn hiện chưa đăng ký tài khoản.");
            }

            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            if (organization == null)
            {
                throw new CustomException("Không tìm thấy tổ chức này.");
            }

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
            var userList = await _userRepository.GetCustomerListByIdList(userIdList);
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
            var currentUser = await _organizationUserRepository.GetOrganizationUserByUserIdAndOrganizationId(currentId, organizationId);

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
            if (removeList.Count <= 0)
            {
                throw new CustomException("Không tồn tại người dùng để xóa khỏi tổ chức.");
            }

            var privateFloorList = await _floorRepository.GetAllPrivateFloorsOfOrganization(organizationId);
            var privateIdList = privateFloorList.Select(p => p.Id).ToList();
            var deleteList = new List<PrivateFloorUser>();
            foreach (var remove in removeList)
            {
                var list = await _privateFloorUserRepository.GetListByUserIdAndPrivateFloorIdList(remove.UserId, privateIdList);
                deleteList.AddRange(list);
            }
            if (deleteList.Count > 0)
            {
                await _privateFloorUserRepository.DeleteRange(deleteList);
            }
            await _organizationUserRepository.DeleteRange(removeList);
        }

        public async Task GrantPrivilege(List<string> userIdList, string organizationId, string currentUserId)
        {
            var ownerId = await _organizationUserRepository.GetOwnerIdOfOrganization(organizationId);
            if (!ownerId.Equals(currentUserId))
            {
                throw new CustomException("Không có quyền thực hiện hành động này.", StatusCodes.Status403Forbidden);
            }

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

        public async Task RemovePrivilege(List<string> userIdList, string organizationId, string currentUserId)
        {
            var ownerId = await _organizationUserRepository.GetOwnerIdOfOrganization(organizationId);
            if (!ownerId.Equals(currentUserId))
            {
                throw new CustomException("Không có quyền thực hiện hành động này.", StatusCodes.Status403Forbidden);
            }

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

        public async Task<OrganizationInfoResponseModel> GetDetailsInfoOfOrganization(string organizationId, string currentUserId)
        {
            var check = await _organizationUserRepository.GetOrganizationUserByUserIdAndOrganizationId(currentUserId, organizationId);
            if (check == null)
            {
                throw new CustomException("Người dùng không thuộc tổ chức này.");
            }

            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            if (organization == null)
            {
                throw new CustomException("Không tồn tại tổ chức này.");
            }
            return _mapper.Map<OrganizationInfoResponseModel>(organization);
        }

        public async Task<List<FloorDetailsInfoResponseModel>> GetFloorListOfOrganization(string organizationId, string currentUserId)
        {
            var publicFloors = await _floorRepository.GetAllPublicFloorsOfOrganization(organizationId);
            var result = new List<FloorDetailsInfoResponseModel>();
            result = _mapper.Map<List<FloorDetailsInfoResponseModel>>(publicFloors);

            var privateFloors = await _floorRepository.GetAllPrivateFloorsOfOrganization(organizationId);
            privateFloors = privateFloors.Where(p => p.Status.Equals(FloorStatusEnums.Active.ToString())).ToList();
            if (privateFloors.Count() > 0)
            {
                var listId = privateFloors.Select(x => x.Id).ToList();
                var privateList = await _privateFloorUserRepository.GetListByUserIdAndPrivateFloorIdList(currentUserId, listId);
                if (privateList.Count() > 0)
                {
                    var privateResult = _mapper.Map<List<FloorDetailsInfoResponseModel>>(privateList.Select(p => p.InteractiveFloor));
                    result = result.Concat(privateResult).ToList();
                }
            }
            return result.OrderBy(r => r.IsPublic).ToList();
        }

        public async Task<string> BuyUserPackageForOrganization(string organizationId, UserPackageOrderCreateRequestModel model, string currentUserId)
        {
            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            if (organization == null)
            {
                throw new CustomException("Không tìm thấy tổ chức này.");
            }
            var newOrder = _mapper.Map<UserPackageOrder>(model);
            newOrder.Id = Guid.NewGuid().ToString();
            newOrder.OrganizationId = organizationId;
            var userPackage = await _userPackageRepository.GetUserPackageById(model.UserPackageId);
            if (userPackage == null)
            {
                throw new CustomException("Không tìm thấy gói nâng cấp thành viên này.");
            }
            newOrder.Price = userPackage.Price;
            newOrder.OrderDate = DateTime.Now;
            newOrder.Status = PackageOrderStatusEnums.PENDING.ToString();
            var payment = await _payosService.CreatePayment(newOrder.Price, model.ReturnUrl, model.CancelUrl);
            if (payment == null)
            {
                throw new CustomException("Có lỗi thanh toán trong hệ thống PayOS.");
            }
            newOrder.OrderCode = payment.orderCode.ToString();
            newOrder.UserId = currentUserId;
            await _userPackageOrderRepository.Insert(newOrder);
            return payment.checkoutUrl;
        }

        public async Task UpdateUserPackageOrderStatus(string orderCode, string currentUserId)
        {
            var order = await _userPackageOrderRepository.GetUserPackageOrderByOrderCode(orderCode);
            if (order == null)
            {
                throw new CustomException("Không tìm thấy đơn hàng này.");
            }

            if (order.Status.Equals(PackageOrderStatusEnums.PAID.ToString()))
            {
                return;
            }
            var paymentInfo = await _payosService.GetPaymentInformation(orderCode);

            if (paymentInfo == null)
            {
                throw new CustomException("Lỗi hệ thống.");
            }
            var newStatus = paymentInfo.status;
            var userPackage = await _userPackageRepository.GetUserPackageById(order.UserPackageId);
            order.Status = newStatus;
            var curUser = await _userRepository.GetUserById(currentUserId);
            if (newStatus.Equals(PackageOrderStatusEnums.PAID.ToString()))
            {
                var htmlBody = HTMLEmailTemplate.PaymentSuccessNotification(curUser.FullName, userPackage.Name, order.OrderDate);
                bool sendEmailSuccess = await _emailService.SendEmail(curUser.Email, "Thông báo mua gói người dùng thành công", htmlBody);
                if (!sendEmailSuccess)
                {
                    throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
                }
                var organization = await _organizationRepository.GetOrganizationById(order.OrganizationId);
                organization.UserLimit += userPackage.UserLimit;
            }
            await _userPackageOrderRepository.Update(order);
        }

        public async Task<List<UserPackageOrderListResponseModel>> GetAllUserPackageOrders(string id)
        {
            var list = await _userPackageOrderRepository.GetAllUserPackageOrderOfOrganization(id);
            var result = list.Select(r => new UserPackageOrderListResponseModel
            {
                Id = r.Id,
                OrderCode = r.OrderCode,
                OrderDate = r.OrderDate,
                PaymentMethod = r.PaymentMethod,
                Price = r.Price,
                Status = r.Status,
                UserPackageId = r.UserPackageId,
                UserPackageInfo = _mapper.Map<UserPackageListResponseModel>(r.UserPackage)
            }).ToList();
            return result;
        }

        public async Task<string> CreateAgainPaymentUrlForPendingUserPackageOrder(string orderId)
        {
            var userPackageOrder = await _userPackageOrderRepository.GetUserPackageOrderById(orderId);

            if (!userPackageOrder.Status.Equals(PackageOrderStatusEnums.PENDING.ToString()))
            {
                throw new CustomException("Giao dịch này không ở trạng thái pending. Không thể tiếp tục thanh toán.");
            }

            var paymentInfo = await _payosService.GetPaymentInformation(userPackageOrder.OrderCode);
            var linkId = paymentInfo.id;
            return $"https://pay.payos.vn/web/{linkId}";
        }

        public async Task<UserPackageOrderDetailsResponseModel> GetUserPackageOrderDetails(string orderId)
        {
            var userPackageOrder = await _userPackageOrderRepository.GetUserPackageOrderById(orderId);
            var result = new UserPackageOrderDetailsResponseModel
            {
                Id = orderId,
                OrderCode = userPackageOrder.OrderCode,
                OrderDate = userPackageOrder.OrderDate,
                PaymentMethod = userPackageOrder.PaymentMethod,
                Price = userPackageOrder.Price,
                Status = userPackageOrder.Status,
                UserPackageInfo = _mapper.Map<UserPackageListResponseModel>(userPackageOrder.UserPackage)
            };

            return result;
        }

        public async Task AutoUpdateUserPackageOrderStatus()
        {
            var now = DateTime.Now;
            var updateList = await _userPackageOrderRepository.GetPendingAndProcessingUserPackageOrder(now);
            foreach (var update in updateList)
            {
                var paymentInfo = await _payosService.GetPaymentInformation(update.OrderCode);
                if (paymentInfo == null)
                {
                    throw new CustomException("Lỗi hệ thống.");
                }
                var newStatus = paymentInfo.status;
                update.Status = newStatus;
            }
            await _userPackageOrderRepository.UpdateRange(updateList);
        }
    }
}
