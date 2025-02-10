using DeliveryApi.Application.DTOs;
using DeliveryApi.Application.Interfaces;
using DeliveryApi.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using Swashbuckle.AspNetCore.Annotations;

namespace DeliveryApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um pedido pelo ID", Description = "Retorna os detalhes de um pedido específico.")]
        [SwaggerResponse(200, "Pedido encontrado", typeof(OrderDto))]
        [SwaggerResponse(404, "Pedido não encontrado")]
        public async Task<IActionResult> Get(string id)
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

        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os pedidos", Description = "Retorna a lista de todos os pedidos cadastrados.")]
        [SwaggerResponse(200, "Lista de pedidos", typeof(IEnumerable<OrderDto>))]
        [SwaggerResponse(404, "Nenhum pedido encontrado")]
        public async Task<IActionResult> GetOrder()
        {
            try
            {
                var orders = await _orderService.GetOrderAsync();
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pedidos");
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação.", Details = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo pedido", Description = "Adiciona um novo pedido ao sistema.")]
        [SwaggerResponse(200, "Pedido criado com sucesso", typeof(OrderDto))]
        [SwaggerResponse(400, "Erro de validação")]
        public async Task<IActionResult> Post([FromBody] OrderDto request, [FromServices] IValidator<OrderDto> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }

                await _orderService.CreateOrderAsync(request);
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido.");
                return StatusCode(500, new { Message = "Ocorreu um erro ao criar o pedido.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um pedido", Description = "Atualiza as informações de um pedido existente.")]
        [SwaggerResponse(204, "Pedido atualizado com sucesso")]
        [SwaggerResponse(400, "ID inválido")]
        public async Task<IActionResult> Put(ObjectId id, [FromBody] OrderDto order)
        {
            try
            {
                if (id.ToString() != order.Id)
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
        [SwaggerOperation(Summary = "Exclui um pedido", Description = "Remove um pedido do sistema pelo ID.")]
        [SwaggerResponse(204, "Pedido excluído com sucesso")]
        [SwaggerResponse(500, "Erro ao excluir o pedido")]
        public async Task<IActionResult> Delete(string id)
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