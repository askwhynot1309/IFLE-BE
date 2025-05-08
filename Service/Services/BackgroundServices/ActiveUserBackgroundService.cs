using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository.Repositories.ActiveUserRepositories;
using Service.Services.ActiveUserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.BackgroundServices
{
    public class ActiveUserBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ActiveUserBackgroundService> _logger;

        public ActiveUserBackgroundService(IServiceProvider serviceProvider, ILogger<ActiveUserBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var activeUserService = scope.ServiceProvider.GetRequiredService<IActiveUserService>();
                        var activeUserRepository = scope.ServiceProvider.GetRequiredService<IActiveUserRepository>();

                        var activeUsers = await activeUserService.GetAllActiveUsers();
                        var currentTime = DateTime.Now;

                        _logger.LogInformation($"Checking {activeUsers.Count} active users at {currentTime}");

                        foreach (var user in activeUsers)
                        {
                            var timeSinceLogin = currentTime - user.LoginTime;
                            _logger.LogInformation($"User {user.UserId} logged in at {user.LoginTime}, time since login: {timeSinceLogin.TotalMinutes} minutes");

                            // If login time + 1 hour has passed, set user as inactive
                            if (user.LoginTime.AddHours(1) <= currentTime)
                            {
                                _logger.LogInformation($"Setting user {user.UserId} as inactive");
                                await activeUserService.UpdateUserActiveStatus(user.UserId, false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking active users");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
} 