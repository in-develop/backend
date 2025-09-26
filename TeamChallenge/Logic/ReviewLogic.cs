using TeamChallenge.Models.Entities;
using TeamChallenge.Repositories;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Requests;
using TeamChallenge.Services;
using TeamChallenge.Helpers;

namespace TeamChallenge.Logic
{
    public class ReviewLogic : IReviewLogic
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ITokenReaderService _tokenReader;
        private readonly ILogger<ReviewLogic> _logger;
        private readonly IRedisCacheService _cache;
        public ReviewLogic(
            RepositoryFactory factory, 
            ILogger<ReviewLogic> logger,
            ITokenReaderService tokenReader, 
            IRedisCacheService cache)
        {
            _reviewRepository = (IReviewRepository)factory.GetRepository<ReviewEntity>();
            _logger = logger;
            _tokenReader = tokenReader;
            _cache = cache;
        }

        public async Task<IResponse> GetAllReviewsAsync()
        {
            try
            {
                var result = await _reviewRepository.GetAllAsync();

                return new GetAllReviewsResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetReviewByIdAsync(int id)
        {
            try
            {
                var result = await _cache.GetValueAsync<ReviewEntity>(id);

                if (result != null)
                {
                    return new GetReviewResponse(result);
                }

                result = await _reviewRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Review with Id = {id} not found");
                }

                await _cache.SetValueAsync(result, id);

                return new GetReviewResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateReviewAsync(CreateReviewRequest requestData)
        {
            var response = _tokenReader.GetUserId();

            if (!response.IsSuccess)
            {
                return response;
            }

            var userId = response.As<GetUserIdResponse>().Data;

            try
            {
                await _reviewRepository.CreateAsync(entity =>
                {
                    entity.Rating = requestData.Rating;
                    entity.Comment = requestData.Comment;
                    entity.UserId = userId;
                    entity.ProductId = requestData.ProductId;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.InnerException?.Message ?? $"Error occured while creating review. User ID : {userId}");
            }
        }

        public async Task<IResponse> UpdateReviewAsync(int id, UpdateReviewRequest requestData)
        {
            var response = _tokenReader.GetUserId();

            if (!response.IsSuccess)
            {
                return response;
            }

            var userId = response.As<GetUserIdResponse>().Data;

            try
            {
                var result = await _reviewRepository.UpdateAsync(id, entity =>
                {
                    entity.Rating = requestData.Rating;
                    entity.Comment = requestData.Comment;
                    entity.UserId = userId;
                    entity.ProductId = requestData.ProductId;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Review with Id = {id} not found");
                }

                await _cache.RemoveValueAsync<ProductEntity>(id);

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.InnerException?.Message ?? $"Error occured while creating review. User ID : {userId}");
            }
        }

        public async Task<IResponse> DeleteReviewAsync(int id)
        {
            try
            {
                var result = await _reviewRepository.DeleteAsync(id);
                if (!result)
                {
                    return new NotFoundResponse($"Review with Id = {id} not found");
                }

                await _cache.RemoveValueAsync<ProductEntity>(id);

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.InnerException?.Message ?? $"Error occured while creating review.");
            }
        }
    }
}
