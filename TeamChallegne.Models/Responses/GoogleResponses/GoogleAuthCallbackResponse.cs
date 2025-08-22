using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GoogleAuthCallbackResponse : BaseDataResponse<GoogleAuthCallback>
    {
        public GoogleAuthCallbackResponse(GoogleAuthCallback data) : base(data)
        {
        }
    }

}
