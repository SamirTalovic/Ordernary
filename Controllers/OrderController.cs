using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ordernary.Data;
using Ordernary.Models;
using Ordernary.Models.DTOs;
using Ordernary.Repositories.Implementation;
using Ordernary.Repositories.Interface;

namespace Ordernary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderInterface _orderRepository;
        private readonly IArticleInterface _articleRepository;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderController(IOrderInterface orderRepository, IHubContext<OrderHub> hubContext, IArticleInterface articleRepository)
        {
            _orderRepository = orderRepository;
            _hubContext = hubContext;
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreteOrderDTO createOrderDto)
        {
            if (createOrderDto == null || createOrderDto.OrderArticles == null || !createOrderDto.OrderArticles.Any())
            {
                return BadRequest("Order data is required.");
            }

            // Validate TableId
            if (createOrderDto.TableId <= 0)
            {
                return BadRequest("Invalid TableId.");
            }

            // Create the order
            var order = new Order
            {
                TableId = createOrderDto.TableId,
                Done = false, // Default value
                OrderArticles = new List<OrderArticle>()
            };

            // Validate and add each article
            foreach (var orderArticleDto in createOrderDto.OrderArticles)
            {
                if (orderArticleDto.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than 0.");
                }

              

                var orderArticle = new OrderArticle
                {
                    ArticleId = orderArticleDto.ArticleId,
                    Quantity = orderArticleDto.Quantity
                };

                order.OrderArticles.Add(orderArticle);
            }

            // Save the order
            await _orderRepository.CreateOrder(order);

            // Notify clients of a new order
            await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", order);

            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrder(id);

            // Notify clients that an order was deleted
            await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate");

            return Ok();
        }
    }
}
