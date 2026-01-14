using Hangfire;
using Microsoft.EntityFrameworkCore;
using TodoApi.Core.Entities; // Adjust namespace if needed
using Microsoft.Extensions.Logging;

namespace TodoApi.Infrastructure.Jobs
{
    public class LicenseExpiryJob
    {
        private readonly AppDBContext _context;
        private readonly ILogger<LicenseExpiryJob> _logger;

        public LicenseExpiryJob(AppDBContext context, ILogger<LicenseExpiryJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Checking for expired licenses...");

            // Find active licenses that have passed their expiry date
            var expiredLicenses = await _context.Licenses
                .Where(l => l.Status == LicenseStatus.Active && l.ExpirationDate < DateTime.UtcNow)
                .ToListAsync();

            foreach (var license in expiredLicenses)
            {
                license.Status = LicenseStatus.Expired;
                _logger.LogInformation($"Expired License: {license.LicenseKey}");
            }

            if (expiredLicenses.Any())
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}