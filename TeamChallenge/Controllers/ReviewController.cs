using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewLogic _reviewLogic;
        public ReviewController(IReviewLogic reviewLogic)
        {
            _reviewLogic = reviewLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reviewLogic.GetAllReviewsAsync();

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _reviewLogic.GetReviewByIdAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest requestData)
        {
            var result = await _reviewLogic.CreateReviewAsync(requestData);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReviewRequest requestData)
        {
            var result = await _reviewLogic.UpdateReviewAsync(id, requestData);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _reviewLogic.DeleteReviewAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
