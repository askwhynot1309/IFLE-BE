using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Ultis
{
    public static class ActivationKeyGenerator
    {
        private static Random random = new Random();

        public static string GenerateKey(int groupCount = 4, int groupLength = 4)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder keyBuilder = new StringBuilder();

            for (int i = 0; i < groupCount; i++)
            {
                if (i > 0)
                    keyBuilder.Append('-');

                for (int j = 0; j < groupLength; j++)
                {
                    keyBuilder.Append(chars[random.Next(chars.Length)]);
                }
            }

            return keyBuilder.ToString();
        }
    }
}
