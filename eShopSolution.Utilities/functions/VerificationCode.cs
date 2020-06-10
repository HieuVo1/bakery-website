using System;

namespace eShopSolution.Utilities.functions
{
    public static class VerificationCode
    {
        public static int GetCode()
        {
            return new Random().Next(900000) + 100000;
        }
    }
}
