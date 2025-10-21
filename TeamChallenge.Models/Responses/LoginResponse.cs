
using TeamChallenge.Models.Login;

namespace TeamChallenge.Models.Responses
{
    public class LoginResponse : BaseDataResponse<LoginResponseModel>
    {
        public LoginResponse(LoginResponseModel data) : base(data)
        {

        }
    }
}
