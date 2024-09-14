using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        private readonly Cloudinary _cloudinary;
        private readonly IArticleInterface _articleRepository;
        public ArticleController(IArticleInterface articleInterface, Cloudinary cloudinary)
        {
            _articleRepository = articleInterface;
            _cloudinary = cloudinary;
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
        public async Task<ActionResult<Article>> PostArticle([FromForm] ArticleDTO articleDto)
        {
            var uploadResult = new ImageUploadResult();

            if (articleDto.Photo != null)
            {
                using (var stream = articleDto.Photo.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(articleDto.Photo.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            var article = new Article
            {
                Name = articleDto.Name,
                Price = articleDto.Price,
                Category = articleDto.Category,
                ImageUrl = uploadResult.SecureUrl.ToString() 
            };

            await _articleRepository.AddAsync(article);
            await _articleRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.ArticleId }, article);
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> PutArticle(int id, [FromForm] ArticleDTO articleDto)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            article.Name = articleDto.Name;
            article.Price = articleDto.Price;
            article.Category = articleDto.Category;
            

            if (articleDto.Photo != null)
            {
                var uploadResult = new ImageUploadResult();
                using (var stream = articleDto.Photo.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(articleDto.Photo.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                article.ImageUrl = uploadResult.SecureUrl.ToString();
            }

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
