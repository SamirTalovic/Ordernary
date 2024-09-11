using Microsoft.AspNetCore.Mvc;
using Ordernary.Models;
using Ordernary.Models.DTOs;
using Ordernary.Repositories.Implementation;
using Ordernary.Repositories.Interface;

namespace Ordernary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleInterface _articleRepository;
        public ArticleController(IArticleInterface articleInterface)
        {
            _articleRepository = articleInterface;
        }
        [HttpGet("allarticles")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var articles = await _articleRepository.GetAllAsync();
            return Ok(articles);
        }
        [HttpGet("article{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }
        [HttpPost("createarticle")]
        public async Task<ActionResult<Article>> PostArticle(ArticleDTO articleDto)
        {
            var article = new Article
            {
                Name = articleDto.Name,
                Price = articleDto.Price
            };

            await _articleRepository.AddAsync(article);
            await _articleRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.ArticleId }, article);
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> PutArticle(int id, ArticleDTO articleDto)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            article.Price = articleDto.Price;
            article.Name = articleDto.Name;

            await _articleRepository.UpdateAsync(article);
            await _articleRepository.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _articleRepository.DeleteAsync(id);
            await _articleRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
