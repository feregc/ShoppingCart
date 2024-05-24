using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Services;

namespace ShoppingCartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        protected ResponseDto _response;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            try
            {
                var list = await _orderService.GetAllOrdersAsync();
                _response.Result = list;
                _response.DisplayMessage = "Orders List";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Order does not exist";
                return NotFound(_response);
            }
            _response.Result = order;
            _response.DisplayMessage = "Order Information";
            return Ok(_response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDto orderDto)
        {
            try
            {
                var model = await _orderService.CreateUpdateOrderAsync(orderDto);
                _response.Result = model;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error ";
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder(OrderDto orderDto)
        {
            try
            {
                var model = await _orderService.CreateUpdateOrderAsync(orderDto);
                _response.Result = model;

                return CreatedAtAction("GetOrder", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error saving register";
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var isDeleted = await _orderService.DeleteOrderAsync(id);
                if (isDeleted)
                {
                    _response.Result = isDeleted;
                    _response.DisplayMessage = "Order was deleted successfully";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Error deleting order";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
