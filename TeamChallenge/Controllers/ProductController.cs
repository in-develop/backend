using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.DTOs.Product;
using TeamChallenge.Models.Models.Responses.CategoryResponse;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.ProductResponses;



namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var product = await _service.GetAllProductsAsync();
                return Ok(new ProductGetAllResponse(product));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById()
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var product = await _service.GetProductByIdAsync(id);
                return product == null ? NotFound(new ErrorResponse($"Your product {id} is not found")) : Ok(new ProductGetByIdResponse(product));
            }
            catch (Exception ex)
            {

                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            try
            {
                var data = await _service.CreateProductAsync(dto);

                if (data != null) return Ok(new ProductCreateResponse(data));
                return NotFound(new ErrorResponse(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var updated = await _service.UpdateProductAsync(id, dto);
                return updated ? Ok(new OkResponse("Product is successfuly updated")) : NotFound(new ErrorResponse("Desired Category is not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var deleted = await _service.DeleteProductAsync(id);
                return deleted ? Ok(new OkResponse("Product is successfuly deleted")) : NotFound(new ErrorResponse("Desired Category is not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}
