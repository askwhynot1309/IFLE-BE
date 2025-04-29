using AutoMapper;
using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.DeviceCategory.Response;
using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.DTOs.GamePackageOrder.Request;
using BusinessObjects.DTOs.GamePackageOrder.Response;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using BusinessObjects.DTOs.InteractiveFloor.Response;
using BusinessObjects.DTOs.SetUpGuide.Request;
using BusinessObjects.DTOs.SetUpGuide.Response;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Repository.Enums;
using Repository.Repositories.DeviceCategoryRepositories;
using Repository.Repositories.DeviceRepositories;
using Repository.Repositories.FloorRepositories;
using Repository.Repositories.FloorUserRepositories;
using Repository.Repositories.GamePackageOrderRepositories;
using Repository.Repositories.GamePackageRepositories;
using Repository.Repositories.OrganizationRepositories;
using Repository.Repositories.OrganizationUserRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.EmailServices;
using Service.Services.PayosServices;
using Service.Ultis;
using System;
using static System.Net.WebRequestMethods;

namespace Service.Services.FloorServices
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceCategoryRepository _deviceCategoryRepository;
        private readonly IPrivateFloorUserRepository _floorUserRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGamePackageRepository _gamePackageRepository;
        private readonly IGamePackageOrderRepository _gamePackageOrderRepository;
        private readonly IPayosService _payosService;
        private readonly IEmailService _emailService;
        private readonly IPrivateFloorUserRepository _privateFloorUserRepository;

        public FloorService(IFloorRepository floorRepository, IMapper mapper, IOrganizationRepository organizationRepository, IDeviceRepository deviceRepository, IDeviceCategoryRepository deviceCategoryRepository, IPrivateFloorUserRepository floorUserRepository, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository, IGamePackageRepository gamePackageRepository, IPayosService payosService, IGamePackageOrderRepository gamePackageOrderRepository, IEmailService emailService, IPrivateFloorUserRepository privateFloorUserRepository)
        {
            _floorRepository = floorRepository;
            _mapper = mapper;
            _organizationRepository = organizationRepository;
            _deviceRepository = deviceRepository;
            _deviceCategoryRepository = deviceCategoryRepository;
            _floorUserRepository = floorUserRepository;
            _organizationUserRepository = organizationUserRepository;
            _userRepository = userRepository;
            _gamePackageRepository = gamePackageRepository;
            _payosService = payosService;
            _gamePackageOrderRepository = gamePackageOrderRepository;
            _emailService = emailService;
            _privateFloorUserRepository = privateFloorUserRepository;
        }

        public async Task CreateFloor(FloorCreateUpdateRequestModel model, string organizationId, string userId)
        {
            var newFloor = _mapper.Map<InteractiveFloor>(model);
            newFloor.Id = Guid.NewGuid().ToString();
            newFloor.Status = FloorStatusEnums.Active.ToString();

            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            if (organization == null)
            {
                throw new CustomException("Không tìm thấy tổ chức này.");
            }

            var nameCheck = await _floorRepository.IsFloorNameExistInOrganization(organizationId, model.Name);
            if (nameCheck)
            {
                throw new CustomException("Tên sàn này đã được bạn dùng trong tổ chức này. Vui lòng đặt tên khác.");
            }
            newFloor.OrganizationId = organizationId;

            await _floorRepository.Insert(newFloor);

            if (model.IsPublic == false)
            {
                await _floorUserRepository.Insert(new PrivateFloorUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FloorId = newFloor.Id,
                    UserId = userId
                });
            }
        }

        public async Task<FloorDetailsInfoResponseModel> ViewFloorDetailInfo(string floorId, string currentUserId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }
            if (floor.IsPublic == false)
            {
                var privateFloorUser = await _privateFloorUserRepository.GetByUserIdAndFloorId(currentUserId, floorId);
                if (privateFloorUser == null)
                {
                    throw new CustomException("Bạn không có quyền truy cập vào sàn tương tác này.");
                }
            }
            var result = new FloorDetailsInfoResponseModel
            {
                Id = floor.Id,
                Name = floor.Name,
                Description = floor.Description,
                Height = floor.Height,
                Length = floor.Length,
                Width = floor.Width,
                IsPublic = floor.IsPublic,
                Status = floor.Status,
                DeviceInfo = floor.Device != null ? _mapper.Map<DeviceInfo>(floor.Device) : null,
            };

            if (floor.Device != null)
            {
                result.DeviceInfo.DeviceCategory = floor.Device.DeviceCategory != null ? _mapper.Map<DeviceCategoryInfoResponseModel>(floor.Device.DeviceCategory) : null;
            }
            return result;
        }

        public async Task SoftRemoveFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }
            var now = DateTime.Now;
            var avalableGamePackageOrderCheck = await _gamePackageOrderRepository.CheckIfAnyAvailableGamePackageInFloor(floorId, now);
            if (avalableGamePackageOrderCheck)
            {
                throw new CustomException("Còn gói game đang sử dụng hoặc chưa kích hoạt trong sàn tương tác này. Không thể xóa sàn.");
            }
            floor.Status = FloorStatusEnums.Inactive.ToString();
            await _floorRepository.Update(floor);
        }

        public async Task UpdateFloor(FloorCreateUpdateRequestModel model, string floorId, string currentUserId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn này.");
            }

            var privilegeList = await _organizationUserRepository.GetOwnerAndCoownerIdListOfOrganization(floor.OrganizationId);
            if (!privilegeList.Contains(currentUserId))
            {
                throw new CustomException("Người dùng này không có quyền cập nhật sàn tương tác", StatusCodes.Status403Forbidden);
            }

            if (!floor.Name.ToLower().Equals(model.Name.ToLower()))
            {
                var nameCheck = await _floorRepository.IsFloorNameExistInOrganization(floor.OrganizationId, model.Name);
                if (nameCheck)
                {
                    throw new CustomException("Tên sàn này đã được bạn dùng trong tổ chức này. Vui lòng đặt tên khác.");
                }
            }

            _mapper.Map(model, floor);
            await _floorRepository.Update(floor);
        }

        public async Task<string> AddDeviceToFloor(string floorId, DeviceCreateUpdateRequestModel model)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này.");
            }

            var newDevice = _mapper.Map<Device>(model);
            newDevice.Id = Guid.NewGuid().ToString();

            bool hasDeviceCategory = !string.IsNullOrEmpty(model.DeviceCategoryId);
            string message;

            var existed = await _deviceRepository.GetDeviceByUri(model.Uri);
            if (existed != null)
            {
                throw new CustomException("Thiết bị với mã định danh này đã được đăng ký cho sàn tương tác khác." +
                    "Vui lòng đăng ký thiết bị khác cho sàn tương tác này.");
            }

            if (hasDeviceCategory)
            {
                var deviceCategory = await _deviceCategoryRepository.GetDeviceCategoryById(model.DeviceCategoryId);
                if (deviceCategory == null)
                {
                    throw new CustomException("Không tìm thấy loại thiết bị này");
                }
                message = "Thêm thiết bị thành công.";
            }
            else
            {
                message = "Thêm thiết bị thành công. Lưu ý: Cần chọn loại thiết bị để chúng tôi biết bạn đang xài loại thiết bị nào nhằm đưa ra hướng dẫn phù hợp.";
            }

            await _deviceRepository.Insert(newDevice);
            floor.DeviceId = newDevice.Id;
            await _floorRepository.Update(floor);

            return message;
        }

        public async Task<DeviceInfo> ViewDeviceInfoOfFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Sàn tương tác này chưa được thêm thiết bị");
            }

            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            if (device == null)
            {
                throw new CustomException("Bạn chưa đăng ký thiết bị cho sàn tương tác này.");
            }
            var result = new DeviceInfo
            {
                Id = device.Id,
                Name = device.Name,
                Description = device.Description,
                Uri = device.Uri,
                DeviceCategory = _mapper.Map<DeviceCategoryInfoResponseModel>(device.DeviceCategory)
            };
            return result;
        }

        public async Task UpdateDeviceInFloor(string floorId, DeviceCreateUpdateRequestModel model)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Sàn tương tác này chưa được thêm thiết bị");
            }

            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            if (device == null)
            {
                throw new CustomException("Không tìm thấy thiết bị này.");
            }

            if (model.Uri != device.Uri)
            {
                var existed = await _deviceRepository.GetDeviceByUri(model.Uri);
                if (existed != null)
                {
                    throw new CustomException("Thiết bị với mã định danh này đã được đăng ký cho sàn tương tác khác." +
                        "Vui lòng đăng ký thiết bị khác cho sàn tương tác này.");
                }
            }

            _mapper.Map(model, device);
            await _deviceRepository.Update(device);
        }

        public async Task RemoveDeviceFromFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Sàn tương tác này chưa được thêm thiết bị");
            }
            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            if (device == null)
            {
                throw new CustomException("Không tìm thấy thiết bị này.");
            }
            floor.DeviceId = null;
            floor.Device = null;

            await _floorRepository.Update(floor);
            await _deviceRepository.Delete(device);
        }

        public async Task AddUserToPrivateFloor(List<string> userIdList, string floorId, string currentUserId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor.IsPublic == true)
            {
                throw new CustomException("Sàn tương tác này không phải riêng tư.");
            }

            var userInOrganization = await _organizationUserRepository.GetUserOfOrganization(floor.OrganizationId);

            bool check = userIdList.All(u => userInOrganization.Contains(u));
            if (!check)
            {
                throw new CustomException("Người dùng không thuộc tổ chức chứa sàn tương tác này.");
            }

            var existedList = await _floorUserRepository.GetListUserIdByFloorId(floorId);
            var duplicateUser = existedList.Intersect(userIdList);
            if (duplicateUser.Count() > 0)
            {
                throw new CustomException("Người dùng đã được thêm vào sàn tương tác riêng tư trước đó.");
            }

            var ownerId = await _organizationUserRepository.GetOwnerIdOfOrganization(floor.OrganizationId);
            if (!ownerId.Equals(currentUserId))
            {
                throw new CustomException("Chỉ có chủ sở hữu sàn tương tác mới được thêm thành viên.", StatusCodes.Status403Forbidden);
            }

            var floorUserList = new List<PrivateFloorUser>();
            foreach (var userId in userIdList)
            {
                floorUserList.Add(new PrivateFloorUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    FloorId = floorId
                });
            }

            await _floorUserRepository.InsertRange(floorUserList);
        }

        public async Task<List<UserInfoResponeModel>> GetUserInPrivateFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor.IsPublic == true)
            {
                throw new CustomException("Sàn tương tác này không phải riêng tư.");
            }
            var userIdList = await _floorUserRepository.GetListUserIdByFloorId(floorId);
            if (userIdList.Count() == 0)
            {
                return null;
            }
            var userList = await _userRepository.GetCustomerListByIdList(userIdList);
            return _mapper.Map<List<UserInfoResponeModel>>(userList.OrderBy(u => u.FullName));
        }

        public async Task RemoveUserFromPrivateFloor(string floorId, List<string> userIdList)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor.IsPublic == true)
            {
                throw new CustomException("Sàn tương tác này không phải riêng tư.");
            }

            var userInOrganization = await _organizationUserRepository.GetUserOfOrganization(floor.OrganizationId);

            bool check = userIdList.All(u => userInOrganization.Contains(u));
            if (!check)
            {
                throw new CustomException("Người dùng không thuộc tổ chức chứa sàn tương tác này.");
            }

            var removeList = await _floorUserRepository.GetListByUserIdList(userIdList, floorId);
            var listUserId = removeList.Select(u => u.UserId).ToList();
            var ownerId = await _organizationUserRepository.GetOwnerIdOfOrganization(floor.OrganizationId);
            if (listUserId.Contains(ownerId))
            {
                throw new CustomException("Không được xóa chủ sở hữu ra khỏi sàn tương tác riêng tư.");
            }

            await _floorUserRepository.DeleteRange(removeList);
        }

        public async Task<string> BuyGamePackageForFloor(string floorId, GamePackageOrderCreateRequestModel model, string currentUserId)
        {
            var newOrder = _mapper.Map<GamePackageOrder>(model);
            newOrder.Id = Guid.NewGuid().ToString();
            newOrder.FloorId = floorId;
            var gamePackage = await _gamePackageRepository.GetGamePackageById(model.GamePackageId);
            if (gamePackage == null)
            {
                throw new CustomException("Không tìm thấy gói game này.");
            }
            newOrder.Price = gamePackage.Price;
            newOrder.OrderDate = DateTime.Now;
            newOrder.Status = PackageOrderStatusEnums.PENDING.ToString();
            var payment = await _payosService.CreatePayment(newOrder.Price, model.ReturnUrl, model.CancelUrl);
            if (payment == null)
            {
                throw new CustomException("Có lỗi thanh toán trong hệ thống PayOS.");
            }
            newOrder.OrderCode = payment.orderCode.ToString();
            newOrder.UserId = currentUserId;
            await _gamePackageOrderRepository.Insert(newOrder);
            return payment.checkoutUrl;
        }

        public async Task<List<GamePackageOrderDetailsResponseModel>> GetAllAvailableGamePackageOfFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Sàn tương tác này không tồn tại.");
            }
            var now = DateTime.Now;
            var availableGamePackageOrder = (await _gamePackageOrderRepository.GetAvailableGamePackage(floorId, now));
            var result = availableGamePackageOrder.Select(a => new GamePackageOrderDetailsResponseModel
            {
                Id = a.Id,
                Price = a.Price,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                OrderDate = a.OrderDate,
                OrderCode = a.OrderCode,
                IsActivated = a.IsActivated,
                PaymentMethod = a.PaymentMethod,
                Status = a.Status,
                GamePackageInfo = new GamePackageDetailsResponseModel
                {
                    Id = a.GamePackage.Id,
                    Name = a.GamePackage.Name,
                    Description = a.GamePackage.Description,
                    Duration = a.GamePackage.Duration,
                    Price = a.GamePackage.Price,
                    Status = a.GamePackage.Status,
                    GameList = _mapper.Map<List<GameInfo>>(a.GamePackage.GamePackageRelations.Select(g => g.Game))
                }
            }).OrderByDescending(o => o.OrderDate).ToList();
            return result;
        }

        public async Task<List<GamePackageOrderDetailsResponseModel>> GetPlayableGamePackageOfFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Sàn tương tác này không tồn tại.");
            }
            var now = DateTime.Now;
            var availableGamePackageOrder = (await _gamePackageOrderRepository.GetPlayableGamePackage(floorId, now));
            var result = availableGamePackageOrder.Select(a => new GamePackageOrderDetailsResponseModel
            {
                Id = a.Id,
                Price = a.Price,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                OrderDate = a.OrderDate,
                OrderCode = a.OrderCode,
                IsActivated = a.IsActivated,
                PaymentMethod = a.PaymentMethod,
                Status = a.Status,
                GamePackageInfo = new GamePackageDetailsResponseModel
                {
                    Id = a.GamePackage.Id,
                    Name = a.GamePackage.Name,
                    Description = a.GamePackage.Description,
                    Duration = a.GamePackage.Duration,
                    Price = a.GamePackage.Price,
                    Status = a.GamePackage.Status,
                    GameList = _mapper.Map<List<GameInfo>>(a.GamePackage.GamePackageRelations.Select(g => g.Game))
                }
            }).ToList();

            return result;
        }

        public async Task UpdateGamePackageOrderStatus(string orderCode, string currentUserId)
        {
            var order = await _gamePackageOrderRepository.GetGamePackageOrderByOrderCode(orderCode);
            if (order == null)
            {
                throw new CustomException("Không tìm thấy đơn hàng này.");
            }
            var paymentInfo = await _payosService.GetPaymentInformation(orderCode);

            if (paymentInfo == null)
            {
                throw new CustomException("Lỗi hệ thống.");
            }
            var newStatus = paymentInfo.status;

            var gamePackage = await _gamePackageRepository.GetGamePackageById(order.GamePackageId);
            order.Status = newStatus;
            var curUser = await _userRepository.GetUserById(currentUserId);
            if (newStatus.Equals(PackageOrderStatusEnums.PAID.ToString()))
            {
                var htmlBody = HTMLEmailTemplate.PaymentSuccessNotification(curUser.FullName, gamePackage.Name, order.OrderDate);
                bool sendEmailSuccess = await _emailService.SendEmail(curUser.Email, "Thông báo mua gói trò chơi thành công", htmlBody);
                if (!sendEmailSuccess)
                {
                    throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
                }
            }
            order.IsActivated = false;
            await _gamePackageOrderRepository.Update(order);
        }

        public async Task ActivateGamePackageOrder(string gamePackageOrderId)
        {
            var order = await _gamePackageOrderRepository.GetGamePackageOrderById(gamePackageOrderId);
            if (order == null)
            {
                throw new CustomException("Không tìm thấy gói game bạn đã mua.");
            }
            if (order.IsActivated.HasValue && order.IsActivated == true)
            {
                throw new CustomException("Gói game này đã được kích hoạt trước đó rồi.");
            }
            order.IsActivated = true;
            order.StartTime = DateTime.Now.Date;

            var gamePackage = await _gamePackageRepository.GetGamePackageById(order.GamePackageId);
            order.EndTime = DateTime.Now.AddMonths(gamePackage.Duration).Date;

            await _gamePackageOrderRepository.Update(order);
        }

        public async Task<List<GamePackageOrderListResponseModel>> GetGamePackageOrderOfFloor(string id)
        {
            var list = await _gamePackageOrderRepository.GetAllGamePackageOrderOfFloor(id);
            var result = list.Select(l => new GamePackageOrderListResponseModel
            {
                Id = l.Id,
                OrderCode = l.OrderCode,
                EndTime = l.EndTime,
                StartTime = l.StartTime,
                IsActivated = l.IsActivated,
                OrderDate = l.OrderDate,
                PaymentMethod = l.PaymentMethod,
                Price = l.Price,
                Status = l.Status,
                GamePackageInfo = new GamePackageDetailsResponseModel
                {
                    Id = l.GamePackage.Id,
                    Name = l.GamePackage.Name,
                    Description = l.GamePackage.Description,
                    Duration = l.GamePackage.Duration,
                    Price = l.GamePackage.Price,
                    Status = l.GamePackage.Status,
                    GameList = _mapper.Map<List<GameInfo>>(l.GamePackage.GamePackageRelations.Select(g => g.Game))
                }
            }).OrderByDescending(o => o.OrderDate).ToList();
            return result;
        }

        public async Task<string> CreateAgainPaymentUrlForPendingGamePackageOrder(string orderId)
        {
            var gamePackageOrder = await _gamePackageOrderRepository.GetGamePackageOrderById(orderId);

            if (gamePackageOrder == null)
            {
                throw new CustomException("Không tìm thấy giao dịch này.");
            }

            if (!gamePackageOrder.Status.Equals(PackageOrderStatusEnums.PENDING.ToString()))
            {
                throw new CustomException("Giao dịch này không trong trạng thái Pending. Không thể tiếp tục thanh toán. Vui lòng tạo order mới.");
            }

            var paymentInfo = await _payosService.GetPaymentInformation(gamePackageOrder.OrderCode);
            var id = paymentInfo.id;
            return $"https://pay.payos.vn/web/{id}";
        }

        public async Task<GamePackageOrderDetailsResponseModel> GetGamePackageOrderDetails(string orderId)
        {
            var gamePackageOrder = await _gamePackageOrderRepository.GetGamePackageOrderById(orderId);
            return new GamePackageOrderDetailsResponseModel
            {
                Id = gamePackageOrder.Id,
                OrderCode = gamePackageOrder.OrderCode,
                EndTime = gamePackageOrder.EndTime,
                StartTime = gamePackageOrder.StartTime,
                IsActivated = gamePackageOrder.IsActivated,
                OrderDate = gamePackageOrder.OrderDate,
                PaymentMethod = gamePackageOrder.PaymentMethod,
                Price = gamePackageOrder.Price,
                Status = gamePackageOrder.Status,
                GamePackageInfo = new GamePackageDetailsResponseModel
                {
                    Id = gamePackageOrder.GamePackage.Id,
                    Name = gamePackageOrder.GamePackage.Name,
                    Description = gamePackageOrder.GamePackage.Description,
                    Duration = gamePackageOrder.GamePackage.Duration,
                    Price = gamePackageOrder.GamePackage.Price,
                    Status = gamePackageOrder.GamePackage.Status,
                    GameList = _mapper.Map<List<GameInfo>>(gamePackageOrder.GamePackage.GamePackageRelations.Select(g => g.Game))
                }
            };
        }

        public async Task AutoUpdateGamePackageOrderStatus()
        {
            var updateList = await _gamePackageOrderRepository.GetPendingAndProcessingGamePackageOrder();

            foreach (var gamePackageOrder in updateList)
            {
                var paymentInfo = await _payosService.GetPaymentInformation(gamePackageOrder.OrderCode);

                if (paymentInfo == null)
                {
                    throw new CustomException("Lỗi hệ thống.");
                }
                gamePackageOrder.Status = paymentInfo.status;
            }

            await _gamePackageOrderRepository.UpdateRange(updateList);

        }

        public async Task AutoActivateGamePackageOrderOver7Days()
        {
            var now = DateTime.Now;

            var orderList = await _gamePackageOrderRepository.GetInactiveGamePackageOrderOver7Days(now);

            foreach (var gamePackageOrder in orderList)
            {
                gamePackageOrder.IsActivated = true;
                gamePackageOrder.StartTime = now.Date;
                var gamePackage = await _gamePackageRepository.GetGamePackageById(gamePackageOrder.GamePackageId);
                gamePackageOrder.EndTime = now.AddMonths(gamePackage.Duration).Date;
            }

            await _gamePackageOrderRepository.UpdateRange(orderList);
        }

        public async Task<SetUpGuideResponseModel> GetSetUpGuideForCustomer(SetUpGuideRequestModel model, string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);

            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này.");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Vui lòng đăng ký thiết bị cho sàn của bạn.");
            }

            if (model.CameraHeight > floor.Height)
            {
                throw new CustomException("Không thể đặt camera cao hơn độ cao phòng.");
            }

            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            if (device.DeviceCategory == null)
            {
                throw new CustomException("Vui lòng chọn loại thiết bị cho thiết bị đã đăng ký.");
            }

            double hFovDeg = device.DeviceCategory.HFov;
            double vFovDeg = device.DeviceCategory.VFov;

            double personHeight = 1f;
            double cameraTiltDeg = model.CameraTiltDeg;
            double zNear;
            double zFar;
            double widthAtZNear;

            double thetaRad = ToRadians(cameraTiltDeg);
            double vfovRad = ToRadians(vFovDeg);
            double hfovRad = ToRadians(hFovDeg);

            double nearAngle = thetaRad + vfovRad / 2.0;
            double dNear = model.CameraHeight / Math.Sin(nearAngle);
            zNear = dNear * Math.Cos(nearAngle);

            double farAngle = thetaRad - vfovRad / 2.0;
            double hHead = model.CameraHeight - personHeight;
            double dFar = hHead / Math.Sin(farAngle);
            zFar = dFar * Math.Cos(farAngle);

            widthAtZNear = dNear * Math.Tan(hfovRad / 2.0);
            var width = 2 * widthAtZNear * (1 - personHeight / dNear);
            var result = new SetUpGuideResponseModel
            {
                DistanceToFloorFromCam = Math.Round(zNear, 2),
                TotalLengthNeeded = Math.Round(zFar, 2),
                PlayFloorLength = Math.Round(zFar - zNear, 2),
                PlayFloorWidth = Math.Round(width, 2),
            };

            if (floor.Width < Math.Round(width, 2))
            {
                result.Notice = $"Độ rộng phần sàn để chơi của bạn là {floor.Width}, bé hơn phần độ rộng cần thiết là {result.PlayFloorWidth}.";
            }

            if (floor.Length < Math.Round(zFar, 2))
            {
                result.Notice = $"Chiều dài phần sàn để chơi của bạn là {floor.Length}, bé hơn phần độ dài cần thiết là {result.TotalLengthNeeded}.";
            }

            return result;
        }

        private double ToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}
