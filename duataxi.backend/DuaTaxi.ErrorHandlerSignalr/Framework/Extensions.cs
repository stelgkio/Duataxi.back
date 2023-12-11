using System;

namespace DuaTaxi.Services.Signalr.Framework
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) 
            => $"users:{userId}";
    }
}