using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Tweetbook.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        //private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RedisHealthCheck(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var _connectionMultiplexer = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IConnectionMultiplexer>();
                
                if (_connectionMultiplexer is null)
                {
                    throw new Exception("Redis caching is turned off...");
                }
                
                var database = _connectionMultiplexer.GetDatabase();
                database.StringGet("health");
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception e)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(e.Message));
            }
        }
    }
}