using WebAPI.Models;

namespace WebAPI.Services
{
    public class LogService
    {
        private readonly EMasterContext _context;

        public LogService(EMasterContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string level, string message)
        {
            var log = new Log
            {
                LogTimeStamp = DateTime.UtcNow,
                LogLevel = level,
                LogMessage = message
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
