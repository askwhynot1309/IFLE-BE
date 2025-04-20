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
    public class PackageOrderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PackageOrderBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var organizationService = scope.ServiceProvider.GetRequiredService<IOrganizationService>();
                    var floorService = scope.ServiceProvider.GetRequiredService<IFloorService>();

                    await organizationService.AutoUpdateUserPackageOrderStatus();
                    await floorService.AutoUpdateGamePackageOrderStatus();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
