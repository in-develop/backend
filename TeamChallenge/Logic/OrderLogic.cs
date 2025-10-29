using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.OrderResponses;
using TeamChallenge.Models.Static_data;
using TeamChallenge.Repositories;
using TeamChallenge.Services;

namespace TeamChallenge.Logic
{
    public class OrderLogic(
        RepositoryFactory factory,
        ILogger<OrderLogic> logger,
        ITokenReaderService tokenReader,
        ICartItemLogic cartItemLogic,
        RepositoryFactory repositoryFactory)
        : IOrderLogic
    {
        private readonly IOrderRepository _orderRepository = (IOrderRepository)factory.GetRepository<OrderEntity>();
        private readonly IOrderItemRepository _orderItemRepository = (IOrderItemRepository)factory.GetRepository<OrderItemEntity>();
        private readonly IOrderHistoryRepository _orderHistoryRepository = (IOrderHistoryRepository)factory.GetRepository<OrderHistoryEntity>();
        private readonly ICartRepository _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
        private readonly IProductRepository _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();

        public async Task<IResponse> CreateOrderAsync()
        {
            return await repositoryFactory.WrapWithTransactionAsync(async () =>
            {
                var response = tokenReader.GetCartId();
                if (!response.IsSuccess)
                {
                    return response;
                }

                var cartId = response.As<GetCartIdResponse>().Data;
                var cart = await _cartRepository.GetCartWithCartItemsAsync(cartId);

                if (cart == null)
                {
                    logger.LogError("Cart not found. ID: {0}", cartId);
                    return new NotFoundResponse($"Cart not found. ID: {cartId}");
                }

                response = tokenReader.GetUserId();
                if (!response.IsSuccess)
                {
                    return response;
                }

                var userId = response.As<GetUserIdResponse>().Data;

                var order = await _orderRepository.CreateAsync(entity =>
                {
                    entity.UserId = userId;
                    entity.Status = OrderStatus.Pending;
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.TotalAmount = CalculateTotalAmountFromCart(cart);
                });

                await _orderItemRepository.CreateManyAsync(cart.CartItems.Count, entities =>
                {
                    for (int i = 0; i < cart.CartItems.Count; i++)
                    {
                        entities[i].OrderId = order.Id;
                        entities[i].ProductId = cart.CartItems[i].ProductId;
                        entities[i].Quantity = cart.CartItems[i].Quantity;
                    }
                });

                await _orderHistoryRepository.CreateAsync(entity =>
                {
                    entity.OrderId = order.Id;
                    entity.NewStatus = OrderStatus.Pending;
                    entity.ChangedAt = DateTime.UtcNow;
                });

                response = await cartItemLogic.DeleteCartItemsFromCartAsync();

                return response;
            });
        }

        private decimal CalculateTotalAmountFromCart(CartEntity cart)
        {
            return cart.CartItems.Sum(item =>
            {
                if (item.Product.DiscountPrice.HasValue)
                {
                    return item.Quantity * item.Product.DiscountPrice.Value;
                }
                else
                {
                    return item.Quantity * item.Product.Price;
                }
            });
        }

        public async Task<IResponse> GetOrderAsync(int orderId)
        {
            try
            {
                var result = await _orderRepository.GetOrderWithDetailsAsync(orderId);

                if (result == null)
                {
                    return new NotFoundResponse($"Order with Id={orderId} not found");
                }

                return new GetOrderResponse(new GetOrderResponseModel(result));
            }
            catch
            {
                return new ServerErrorResponse("An error occurred while retrieving the order.");
            }
        }

        public async Task<IResponse> DeleteOrderAsync(int orderId)
        {
            return await repositoryFactory.WrapWithTransactionAsync<IResponse>(async () =>
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

                if (order == null)
                {
                    return new NotFoundResponse($"Order with Id={orderId} not found");
                }

                var lastOrderHistory = order.OrderHistory.OrderByDescending(x => x.ChangedAt).FirstOrDefault();

                if (lastOrderHistory == null)
                {
                    return new NotFoundResponse($"Order history not found for order : {order.Id}");
                }

                var result = await _orderRepository.UpdateAsync(order.Id, entity =>
                {
                    entity.Status = OrderStatus.Cancelled;
                });

                await _orderHistoryRepository.CreateAsync(entity =>
                {
                    entity.OrderId = order.Id;
                    entity.OldStatus = lastOrderHistory.NewStatus;
                    entity.NewStatus = OrderStatus.Cancelled;
                    entity.ChangedAt = DateTime.UtcNow;
                });

                return new OkResponse();
            });
        }

        public async Task<IResponse> SubmitOrderAsync(int orderId)
        {
            return await repositoryFactory.WrapWithTransactionAsync<IResponse>(async () =>
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

                if (order == null)
                {
                    return new NotFoundResponse($"Order with Id={orderId} not found");
                }

                if (order.Status == OrderStatus.Cancelled)
                {
                    return new BadRequestResponse($"Cannot submit a cancelled order. ID : {orderId}");
                }

                var result = await _orderRepository.UpdateAsync(order.Id, entity =>
                {
                    entity.Status = OrderStatus.Processing;
                });

                await _orderHistoryRepository.CreateAsync(entity =>
                {
                    entity.OrderId = order.Id;
                    entity.OldStatus = OrderStatus.Pending;
                    entity.NewStatus = OrderStatus.Processing;
                    entity.ChangedAt = DateTime.UtcNow;
                });

                var productQuantityDict = order.OrderItems.ToDictionary(x => x.ProductId, x => x.Quantity);

                result = await _productRepository.UpdateManyAsync(
                    p => productQuantityDict.ContainsKey(p.Id),
                    products =>
                    {
                        foreach (var product in products)
                        {
                            product.StockQuantity -= productQuantityDict[product.Id];
                        }
                    });

                if (!result)
                {
                    return new ServerErrorResponse("Failed to update product stock quantities.");
                }

                return new OkResponse();
            });
        }
    }
}
