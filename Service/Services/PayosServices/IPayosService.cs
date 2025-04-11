using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.PayosServices
{
    public interface IPayosService
    {
        Task<CreatePaymentResult> Create(decimal price, string returnUrl, string cancelUrl);

        Task<PaymentLinkInformation> GetPaymentInformation(string orderCode);
    }
}
