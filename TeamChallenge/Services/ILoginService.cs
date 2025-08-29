using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests.Login;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public interface ILoginService
    {
        Task<IResponse> Login(TCLoginRequest request);
        Task<IResponse> SignUp(SignUpRequest request);
        Task<IResponse> Logout();
        Task<IResponse> ConfirmEmail(string userId, string code);
        Task<IResponse> ResendEmailConfirmation(string email, string clientUrl);
    }
}
