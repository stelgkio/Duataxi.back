using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace DuaTaxi.Common
{
    public static class Extensions
    {
        public static string Underscore(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);
            
            return model;
        }

        public static Guid ToGuid(this String str)
        {                                      
            return Guid.Parse(str);
        }
    }
}