using ExampleAPI.Entities;
using ExampleAPI.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StockController : ControllerBase
	{
		private IStockRepository _stockRepository;

		public StockController(IStockRepository stockRepository)
		{
			_stockRepository = stockRepository;
		}

		[HttpGet("GetAll")]
		public IActionResult GetAll()
		{
			return Ok(_stockRepository.GetAll());
		}

		[HttpGet("GetAllWithOrders")]
		public IActionResult GetAllWithOrders()
		{
			return Ok(_stockRepository.GetAll(include: stock => stock.Include(c => c.Order)));
		}

		[HttpGet("GetById/{id}")]
		public IActionResult Get(Guid id)
		{
			return Ok(_stockRepository.Get(order => order.Id == id));
		}

		[HttpPost("Add")]
		public IActionResult Add([FromBody] Stock stock)
		{
			return Ok(_stockRepository.Add(stock));
		}

		[HttpPut("Update")]
		public IActionResult Update([FromBody] Stock stock)
		{
			return Ok(_stockRepository.Update(stock));
		}

		[HttpDelete("DeleteById/{id}")]
		public IActionResult Delete(Guid id)
		{
			var stock = _stockRepository.Get(stock => stock.Id == id);
			if (stock == null) return BadRequest("Stock not found");
			return Ok(_stockRepository.Delete(stock));
		}
	}
}
