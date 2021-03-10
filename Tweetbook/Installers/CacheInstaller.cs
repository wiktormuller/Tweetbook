using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetbook.Options;
using Tweetbook.Services;

namespace Tweetbook.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheOptions = new RedisCacheOptions();
            configuration.GetSection(nameof(RedisCacheOptions)).Bind(redisCacheOptions);
            services.AddSingleton(redisCacheOptions);

            if (!redisCacheOptions.Enabled)
            {
                return;
            }

            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheOptions.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}