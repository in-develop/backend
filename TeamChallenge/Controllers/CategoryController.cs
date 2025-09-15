using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryLogic _categoryLogic;

        public CategoryController(ICategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        public async Task<IResponse> GetAll()
        {
<<<<<<< HEAD
            return await _categoryLogic.GetAllCategoriesAsync();
=======
            return await _productLogic.GetAllCategoriesAsync();
>>>>>>> master
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
<<<<<<< HEAD
            return await _categoryLogic.GetCategoryByIdAsync(id);
        }

        [HttpPost("create")]
        public async Task<IResponse> Create([FromBody] CreateCategoryRequest requestData)
        {
            return await _categoryLogic.CreateCategoryAsync(requestData);
=======
            return await _productLogic.GetCategoryByIdAsync(id);
        }

        [HttpPost]
        public async Task<IResponse> Create([FromBody] CreateCategoryRequest requestData)
        {
            return await _productLogic.CreateCategoryAsync(requestData);
>>>>>>> master
        }

        [HttpPut("{id}")]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateCategoryRequest requestData)
        {
<<<<<<< HEAD
            return await _categoryLogic.UpdateCategoryAsync(id, requestData);
        }

        [HttpDelete("{id}")]
         public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _categoryLogic.DeleteCategoryAsync(id);
=======
            return await _productLogic.UpdateCategoryAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _productLogic.DeleteCategoryAsync(id);
>>>>>>> master
        }
    }
}