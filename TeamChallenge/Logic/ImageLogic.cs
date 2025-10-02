using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class ImageLogic : IImageLogic
    {
        private readonly IProductLogic _productLogic;
        public readonly IImageRepository _imageRepository;
        private readonly ILogger<ImageLogic> _logger;

        public ImageLogic(RepositoryFactory factory, IProductLogic productLogic, ILogger<ImageLogic> logger)
        {
            _imageRepository = (IImageRepository)factory.GetRepository<ImageEntity>();
            _productLogic = productLogic;
            _logger = logger;
        }

        public async Task<IResponse> DeleteImageAsync(int id)
        {
            try
            {
                var image = await _imageRepository.GetByIdAsync(id);
                if (image == null)
                {
                    return new NotFoundResponse($"Image with this id is not exist: {id}");
                }

                var isSuccess = await _imageRepository.DeleteAsync(id);
                if (!isSuccess)
                {
                    return new ServerErrorResponse($"Failed to delete image with id: {id}");
                }

                return new OkResponse();

            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while deleting with id: {id}\nImage: {ex.Message}");
            }
        }

        public async Task<IResponse> GetAllImagesAsync()
        {
            try
            {
                var images = await _imageRepository.GetAllAsync();
                var result = new GetImageListResponse(images);

                return result;
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while getting all images: {ex.Message}");
            }
        }

        public async Task<IResponse> GetImageAsync(int id)
        {
            try
            {
                var image = await _imageRepository.GetByIdAsync(id);
                if (image == null)
                {
                    return new NotFoundResponse($"Image with this id is not exist: {id}");
                }

                var result = new GetImagesResponse(image);

                return result;

            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while getting image by id: {ex.Message}");
            }
        }

        public async Task<IResponse> GetImagesByProductIdAsync(int productId)
        {
            var response = await _productLogic.CheckIfProductsExists(productId);
            if (!response.IsSuccess)
            {
                return response;
            }

            var images = await _imageRepository.GetFilteredAsync(x => x.ProductId == productId);
            var result = new GetImageListResponse(images);

            return result;
        }

        public async Task<IResponse> UpdateImageAsync(UpdateImageRequest dto)
        {
            try
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    if (dto.ImageFile == null)
                    {
                        _logger.LogError("No file uploaded.");
                        return new BadRequestResponse("No file uploaded.");
                    }

                    await dto.ImageFile.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                var base64 = Convert.ToBase64String(fileBytes);

                var q = await _imageRepository.UpdateAsync(dto.Id, x =>
                {
                    x.ImageBase64 = base64;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while updating image: {ex.Message}");
            }
        }

        public async Task<IResponse> AddImageAsync(CreateImageRequest dto)
        {
            try
            {
                var isProductExist = await _productLogic.CheckIfProductsExists(dto.ProductId);

                if (!isProductExist.IsSuccess)
                {
                    return isProductExist;
                }

                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    if (dto.ImageFile == null)
                    {
                        _logger.LogError("No file uploaded.");
                        return new BadRequestResponse("No file uploaded.");
                    }

                    await dto.ImageFile.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                var base64 = Convert.ToBase64String(fileBytes);

                if (dto.ImageFile == null)
                {
                    _logger.LogError("No file uploaded.");
                    return new BadRequestResponse("No file uploaded.");
                }

                await _imageRepository.CreateAsync(entity =>
                {
                    entity.ImageBase64 = base64;
                    entity.ProductId = dto.ProductId;
                });                

                return new OkResponse();

            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while uploading error existence: {ex.Message}");
            }

        }
    }
}
