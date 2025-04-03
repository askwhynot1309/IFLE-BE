using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.RefreshTokenRepositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string refreshToken)
        {
            return await GetSingle(r => r.Token.Equals(refreshToken));
        }
    }
}
