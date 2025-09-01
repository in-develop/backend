﻿using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public interface ICartRepository : IRepository<CartEntity>
    {
        Task<bool> IsCartExist(string UserId);
    }
}
