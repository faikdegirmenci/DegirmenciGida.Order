using Application;
using Application.Response;
using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Commands.Order.Delete;
using DegirmenciGida.Order.Application.Queries.Order.GetById;
using DegirmenciGida.Order.Application.Queries.Order.GetList;
using Infrastructure.LoggingServices;
using Microsoft.AspNetCore.Mvc;

namespace DegirmenciGida.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderCommand command)
        {
            LoggingService.LogInformation("OrderController Post method CreateOrder called!");
            GenericServiceResponse<CreateOrderCommandResponse> response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("CreateOrderRabbitMQ")]
        public async Task<IActionResult> CreateOrderRabbitMQ([FromBody] CreateOrderRabbitMqCommand command)
        {
            LoggingService.LogInformation("OrderController Post method CreateOrderRabbitMQ called!");

            GenericServiceResponse<CreateOrderCommandResponse> response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            LoggingService.LogInformation("OrderController delete method DeleteOrder called!");

            DeleteOrderCommand command = new DeleteOrderCommand() { Id=id};
            GenericServiceResponse<DeleteOrderCommandResponse> response = await Mediator.Send(command);
            return Ok(response);
        }
        [HttpGet("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder([FromQuery] PageRequest request)
        {
            LoggingService.LogInformation("OrderController get method GetAllOrder called!");

            GetAllOrdersQuery query = new GetAllOrdersQuery() { PageRequest = request};
            GetListResponse<GetAllOrderResponse> response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            LoggingService.LogInformation("OrderController get method GetOrderById called!");

            GetOrderByIdQuery query = new GetOrderByIdQuery() { Id = id};
            GenericServiceResponse<GetOrderByIdResponse> response = await Mediator.Send(query);
            return Ok(response);
        }
        //[HttpPut("UpdateOrder")]
        //public async Task<GenericServiceResponse<string>> UpdatedOrder([FromBody]UpdateOrder request)
        //{
        //    UpdatedOrderCommand updatedOrderOp = new UpdatedOrderCommand(_orderService);
        //    return await updatedOrderOp.Execute(request);
        //}
    }
}
