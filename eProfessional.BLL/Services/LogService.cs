using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.BLL.Interfaces;
using eProfessional.DAL.Interfaces;

namespace eProfessional.BLL.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public LogService(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public void CreateLog(string level, string message)
        {
            _logRepository.CreateLog(level, message);
        }

        public IEnumerable<LogDto> GetLogs(int count, int start)
        {
            var logs = _logRepository.GetLogs(count, start);
            return _mapper.Map<IEnumerable<LogDto>>(logs);
        }

        public int GetLogCount()
        {
            return _logRepository.GetCount();

        }
    }
}
