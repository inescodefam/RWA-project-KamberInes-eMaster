using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using eProfessional.DAL.Models;

namespace eProfessional.DAL.Repositories
{
    public class LogRepository : CrudRepository<Log>, ILogRepository
    {
        public LogRepository(EProfessionalContext context) : base(context)
        { }

        public void CreateLog(string level, string message)
        {
            var log = new Log
            {
                LogTimeStamp = DateTime.UtcNow,
                LogLevel = level,
                LogMessage = message
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public IEnumerable<Log> GetLogs(int count, int start)
        {
            return _context.Logs.Skip(start * count).Take(count).ToList();
        }

        public int GetCount()
        {
            return _context.Logs.Count();

        }
    }
}
