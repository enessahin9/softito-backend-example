using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private IOrderRepository _orderRepository;

		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		[HttpGet("GetAll")]
		public IActionResult GetAll()
		{
			return Ok(_orderRepository.GetAll());
		}

		[HttpGet("GetAllWithStocks")]
		public IActionResult GetAllWithStocks()
		{
			return Ok(_orderRepository.GetAll(include: order => order.Include(c => c.Stocks)));
		}

		[HttpGet("GetById/{id}")]
		public IActionResult Get(Guid id)
		{
			return Ok(_orderRepository.Get(order => order.Id == id));
		}

		[HttpPost("Add")]
		public IActionResult Add([FromBody] Order order)
		{
			return Ok(_orderRepository.Add(order));
		}

		[HttpPut("Update")]
		public IActionResult Update([FromBody] Order order)
		{
			var stockItem = _orderRepository.Get(s => s.ProductId == order.ProductId);
			if (stockItem != null && stockItem.Quantity >= order.Quantity)
			{
				stockItem.Quantity -= order.Quantity;
				return Ok(_orderRepository.Update(stockItem));
			}
			return BadRequest("This is not possible");
		}

		[HttpDelete("DeleteById/{id}")]
		public IActionResult Delete(Guid id)
		{
			var order = _orderRepository.Get(order => order.Id == id);
			if (order == null) return BadRequest("Order not found");
			return Ok(_orderRepository.Delete(order));
		}
	}
}
