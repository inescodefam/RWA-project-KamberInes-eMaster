using AutoMapper;
using eProfessional.BLL.DTOs;
using eProfessional.DAL.Interfaces;

namespace eProfessional.BLL.Services
{
    public class LogService
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

        public IEnumerable<LogDto> GetLogs(int count, int start = 0)
        {
            var logs = _logRepository.GetLogs(count, start);
            return _mapper.Map<IEnumerable<LogDto>>(logs);
        }
    }
}
