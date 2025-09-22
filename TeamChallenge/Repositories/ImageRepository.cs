using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ImageRepository : BaseRepository<ImageEntity>, IImageRepository
    {

        public ImageRepository(CosmeticStoreDbContext context,
            ILogger<IRepository<ImageEntity>> logger) : base(context, logger)
        {
        }
    }
}
