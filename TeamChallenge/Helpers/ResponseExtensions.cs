using TeamChallenge.Models.Responses;

namespace TeamChallenge.Helpers
{
    public static class ResponseExtensions
    {
        /// <summary>
        /// Converts IResponse to a specific response type
        /// Use only if response is succesful!
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static T As<T>(this IResponse response) where T : class, IResponse
        {
            return response as T;

        }
    }
}
