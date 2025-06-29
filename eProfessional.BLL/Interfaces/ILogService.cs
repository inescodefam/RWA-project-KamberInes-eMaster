using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface ILogService
    {
        void CreateLog(string level, string message);
        IEnumerable<LogDto> GetLogs(int count, int start = 0);
        int GetLogCount();
    }
}
