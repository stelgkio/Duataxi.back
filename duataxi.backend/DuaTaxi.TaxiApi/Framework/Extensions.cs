using System;

namespace DuaTaxi.Services.TaxiApi.Framework
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) 
            => $"users:{userId}";
    }
}