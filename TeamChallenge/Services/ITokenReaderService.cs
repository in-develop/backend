using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public interface ITokenReaderService
    {
        IResponse GetCartId();
        IResponse GetUserId();
    }

}
