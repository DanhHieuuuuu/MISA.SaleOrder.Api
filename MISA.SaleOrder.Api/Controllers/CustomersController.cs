using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Dtos;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Service;
using MySqlConnector;

namespace MISA.SaleOrder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        [HttpGet("get-all")]
        public IActionResult Get()
        {
            // Gọi service xử lí nghiệp vụ
            var res = _customerService.GetAll();
            // Trả thông tin về cho client
            return Ok(res);

        }

        [HttpGet("paging")]
        public IActionResult Paging(FilterCustomerDto filter)
        {
            // Gọi service xử lí nghiệp vụ
            var res = _customerService.Paging(filter);
            // Trả thông tin về cho client
            return Ok(res);

        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] Customer customer)
        {
            // Gọi service xử lí nghiệp vụ
            var res = _customerService.Insert(customer);
            // Trả thông tin về cho client
            return Ok(res);
        }

        [HttpPut("update")]
        public IActionResult Put([FromBody] Customer customer)
        {
            // Gọi service xử lí nghiệp vụ
            var res = _customerService.Update(customer);
            // Trả thông tin về cho client
            return Ok(res);
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] Guid id)
        {
            // Gọi service xử lí nghiệp vụ
            var res = _customerService.Delete(id);
            // Trả thông tin về cho client
            return Ok(res);
        }
    }
}
