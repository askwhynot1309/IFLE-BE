using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Services.FloorServices;
using Service.Services.OrganizationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.BackgroundServices
{
    public class GamePackageActivationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public GamePackageActivationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var floorService = scope.ServiceProvider.GetRequiredService<IFloorService>();

                    await floorService.AutoActivateGamePackageOrderOver7Days();
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
