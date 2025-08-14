
using TeamChallenge.Models.Login;

namespace TeamChallenge.Models.Responses
{
    public class LoginResponse : BaseDataResponse<LoginModel>
    {
        public LoginResponse(LoginModel data) : base(data)
        {

        }
    }
}
