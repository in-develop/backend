using TeamChallenge.Models.Entities;
using TeamChallenge.Repositories;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Requests;

namespace TeamChallenge.Logic
{
    public class ReviewLogic : IReviewLogic
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewLogic(RepositoryFactory factory)
        {
            _reviewRepository = (IReviewRepository)factory.GetRepository<ReviewEntity>();
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
                    return new NotFoundResponse($"Product with Id={id} not found");
                }

                return new GetReviewResponse(result);
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> CreateReviewAsync(CreateReviewRequest requestData)
        {
            try
            {
                await _reviewRepository.CreateAsync(entity =>
                {
                    entity.Rating = requestData.Rating;
                    entity.Comment = requestData.Comment;
                    entity.UserId = requestData.UserId;
                    entity.ProductId = requestData.ProductId;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> UpdateReviewAsync(int id, UpdateReviewRequest requestData)
        {
            try
            {
                var result = await _reviewRepository.UpdateAsync(id, entity =>
                {
                    entity.Rating = requestData.Rating;
                    entity.Comment = requestData.Comment;
                    entity.UserId = requestData.UserId;
                    entity.ProductId = requestData.ProductId;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Product with Id={id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> DeleteReviewAsync(int id)
        {
            try
            {
                var result = await _reviewRepository.DeleteAsync(id);
                if (!result)
                {
                    return new NotFoundResponse($"Product with Id={id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse(ex.Message);
            }
        }
    }
}
