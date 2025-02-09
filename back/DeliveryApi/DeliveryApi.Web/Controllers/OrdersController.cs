﻿using DeliveryApi.Application.DTOs;
using DeliveryApi.Application.Services;
using DeliveryApi.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DeliveryApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pedido por ID.");

                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderRequest request, [FromServices] IValidator<CreateOrderRequest> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }

                var orderItems = request.Items.Select(item => new OrderItem(item.ProductName, item.Quantity, item.Price)).ToList();
                await _orderService.CreateOrderAsync(request.CustomerName, request.Status, orderItems);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido.");
                return StatusCode(500, new { Message = "Ocorreu um erro ao criar o pedido.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    return BadRequest();
                }

                await _orderService.UpdateOrderAsync(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pedido.");

                return StatusCode(500, new { Message = "Ocorreu um erro ao atualizar o pedido.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir pedido.");

                return StatusCode(500, new { Message = "Ocorreu um erro ao excluir o pedido.", Details = ex.Message });
            }
        }
    }
}