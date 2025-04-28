using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.PayosServices
{
    public class PayosService : IPayosService
    {
        private readonly IConfiguration _configuration;

        public PayosService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private PayOS CreatePayOS()
        {
            var clientId = _configuration["PayOS:CLIENT_ID"];
            var apiKey = _configuration["PayOS:API_KEY"];
            var checksumKey = _configuration["PayOS:CHECKSUM_KEY"];

            var payOS = new PayOS(clientId, apiKey, checksumKey);
            return payOS;
        }

        public async Task<CreatePaymentResult> CreatePayment(decimal price, string returnUrl, string cancelUrl)
        {
            try
            {
                var payOS = CreatePayOS();
                var priceInt = (int)Math.Round(price);

                var paymentLinkRequest = new PaymentData(
                    orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                    amount: priceInt,
                    description: "Mua gói dịch vụ.",
                    items: [new("Mua gói dịch vụ", 1, priceInt)],
                    returnUrl: returnUrl,
                    cancelUrl: cancelUrl,
                    expiredAt: DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
                );
                var response = await payOS.createPaymentLink(paymentLinkRequest);
                return response;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<PaymentLinkInformation> GetPaymentInformation(string orderCode)
        {
            try
            {
                var key = long.Parse(orderCode);
                var payOS = CreatePayOS();
                var response = await payOS.getPaymentLinkInformation(key);
                return response;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

    }
}
