namespace TeamChallenge.Services
{
    public interface IRedisCacheService
    {
        Task<T?> GetValueAsync<T>(int id) where T : class;
        Task<T?> GetValueAsync<T>(string id) where T : class;
        Task SetValueAsync<T>(T data, int id);
        Task SetValueAsync<T>(T data, string id);
        Task RemoveValueAsync<T>(int id);
        Task RemoveValueAsync<T>(string id);

    }

}
