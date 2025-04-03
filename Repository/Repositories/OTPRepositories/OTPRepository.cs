using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.OTPRepositories
{
    public class OTPRepository : GenericRepository<OTP>, IOTPRepository
    {
        public OTPRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<OTP?> GetLatestOTP(string userId)
        {
            var latestOTPList = await Get(o => o.UserId == userId, o => o.OrderByDescending(o => o.CreatedAt));
            return latestOTPList.FirstOrDefault();
        }
    }
}
