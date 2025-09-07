using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CategoryResponses;
using TeamChallenge.Models.Responses.SubCategoryResponses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/subcategories")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryLogic _subCategoryLogic;

        public SubCategoryController(ISubCategoryLogic subCategoryLogic)
        {
            _subCategoryLogic = subCategoryLogic;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllSubCategoriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResponse> GetAll()
        {
            return await _subCategoryLogic.GetAllSubCategoryAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetSubCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await _subCategoryLogic.GetSubCategoryByIdAsync(id);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateSubCategoryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResponse> Create([FromBody] CreateSubCategoryRequest requestData)
        {
            return await _subCategoryLogic.CreateSubCategoryAsync(requestData);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OkResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateSubCategoryRequest requestData)
        {
            return await _subCategoryLogic.UpdateSubCategoryAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(OkResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _subCategoryLogic.DeleteSubCategoryAsync(id);
        }
    }
}
