using eProfessional.BLL.DTOs;

namespace eProfessional.BLL.Interfaces
{
    public interface IAuthService
    {
        public bool RegisterUser(AuthDto userAuthDto);
        public List<string> LoginUser(AuthDto userAuthDto);
    }
}
