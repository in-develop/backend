using Humanizer;
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
                
                File.Delete(image.ImageUrl!);
                
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
                return new GetAllImagesResponse(images);
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
                return new GetImageResponse(image);

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

            var images = await _imageRepository.GetFilteredAsync(x=> x.ProductId == productId);
            return new GetAllImagesResponse(images);
        }

        public async Task<IResponse> UpdateImageAsync(UpdateImageRequest dto)
        {
            try
            {
                var existingImage = await _imageRepository.GetByIdAsync(dto.Id);
                if (existingImage == null)
                {
                    return new NotFoundResponse($"Image with this id is not exist: {dto.Id}");
                }

                var imageUrl = existingImage.ImageUrl!;
                File.Delete(imageUrl);// nameCopy2

                using (var stream = new FileStream(imageUrl, FileMode.Create))
                {
                    await dto.ImageFile!.CopyToAsync(stream);
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while updating image: {ex.Message}");
            }
        }

        public async Task<IResponse> UploadImageAsync(CreateImageRequest dto)
        {
            try
            {
                var isProductExist = await _productLogic.CheckIfProductsExists(dto.ProductId);

                if (!isProductExist.IsSuccess)
                {
                    return isProductExist;
                }

                if (dto.ImageFile == null)
                {
                    _logger.LogError("No file uploaded.");
                    return new BadRequestResponse("No file uploaded.");
                }

                var path = Path.Combine("D:\\CosmeticImages", $"{dto.ProductId}To{Guid.NewGuid().ToString()}.png");

                await _imageRepository.CreateAsync(entity =>
                {
                    entity.ImageUrl = path;
                    entity.ProductId = dto.ProductId;
                });

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                return new OkResponse();

            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Error while uploading error existence: {ex.Message}");
            }

        }
    }
}
