using eProfessional.DAL.Models;

namespace eProfessional.DAL.Interfaces
{
    public interface ILogRepository : ICrudRepository<Log>
    {
        public void CreateLog(string level, string message);
        public IEnumerable<Log> GetLogs(int count, int start);
        public int GetCount();

    }
}
