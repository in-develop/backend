namespace TeamChallenge.Logic
{
    public interface IUserLogic
    {
        Task<bool> CheckIfUserExists(string id);
    }
}
