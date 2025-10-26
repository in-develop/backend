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
        public ReviewLogic(
            RepositoryFactory factory, 
            ITokenReaderService tokenReader)
        {
            _reviewRepository = (IReviewRepository)factory.GetRepository<ReviewEntity>();
            _tokenReader = tokenReader;
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
                var result = await _reviewRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return new NotFoundResponse($"Review with Id = {id} not found");
                }
                
                var review = new GetReviewResponseModel(result);

                return new GetReviewResponse(review);
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

                return new OkResponse($"Review with Id = {id} updated");
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

                return new OkResponse($"Review with Id = {id} deleted");
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.InnerException?.Message ?? $"Error occured while creating review.");
            }
        }
    }
}
