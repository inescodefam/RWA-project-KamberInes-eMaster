using Shared.BL.Models;
using WebAPI.Context;

namespace WebAPI.Services
{
    public class LogService
    {
        private readonly EProfessionalContext _context;

        public LogService(EProfessionalContext context)
        {
            _context = context;
        }

        public void Log(string level, string message)
        {
            var log = new Log
            {
                LogTimeStamp = DateTime.UtcNow,
                LogLevel = level,
                LogMessage = message
            };

            _context.Logs.Add(log);
            _context.SaveChangesAsync();
        }
    }
}
