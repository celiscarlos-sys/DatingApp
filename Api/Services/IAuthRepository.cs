using Api.Dtos.Auth;

namespace Api.Services
{
    public interface IAuthRepository
    {
        bool registerUser(UserToRegister userToRegister);
        UserToLoginResponse loginUser(UserToLogin userToLogin);
    }
}