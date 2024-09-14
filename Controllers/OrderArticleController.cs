using Microsoft.AspNetCore.Mvc;
using Ordernary.Repositories.Implementation;
using Ordernary.Repositories.Interface;

namespace Ordernary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderArticleController : ControllerBase
    {
        private readonly IOrderArticleInterface _orderArticleRepository;

        public OrderArticleController(IOrderArticleInterface orderArticleRepository)
        {
            _orderArticleRepository = orderArticleRepository;
        }

        // Get all OrderArticles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderArticles = await _orderArticleRepository.GetAllAsync();
            return Ok(orderArticles);
        }

        // Get OrderArticle by OrderId and ArticleId
        [HttpGet("{orderId}/{articleId}")]
        public async Task<IActionResult> GetById(int orderId, int articleId)
        {
            var orderArticle = await _orderArticleRepository.GetByIdAsync(orderId, articleId);
            if (orderArticle == null)
            {
                return NotFound();
            }

            return Ok(orderArticle);
        }

        // Delete OrderArticle by OrderId and ArticleId
        [HttpDelete("{orderId}/{articleId}")]
        public async Task<IActionResult> Delete(int orderId, int articleId)
        {
            var orderArticle = await _orderArticleRepository.GetByIdAsync(orderId, articleId);
            if (orderArticle == null)
            {
                return NotFound();
            }

            await _orderArticleRepository.DeleteAsync(orderId, articleId);
            return NoContent();
        }
    }
}
