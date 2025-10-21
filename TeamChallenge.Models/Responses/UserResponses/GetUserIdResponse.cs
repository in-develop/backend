namespace TeamChallenge.Models.Responses
{
    public class GetUserIdResponse : BaseDataResponse<string>
    {
        public GetUserIdResponse(string data) : base(data)
        {
        }
    }
}
