using Application;
using DegirmenciGida.Order.Application.Commands.OrderDetails.Delete;
using DegirmenciGida.Order.Application.Queries.OrderDetails.GetById;
using DegirmenciGida.Order.Application.Queries.OrderDetails.GetList;
using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Domain;
using Microsoft.AspNetCore.Mvc;
using Application.Response;
using DegirmenciGida.Order.Application.Commands.OrderDetails.Update;
using Infrastructure.LoggingServices;

namespace DegirmenciGida.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController:BaseController
    {

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail([FromRoute] Guid id)
        {
            LoggingService.LogInformation("OrderDetailController delete method DeleteOrderDetail called!");

            DeleteOrderDetailCommand command = new DeleteOrderDetailCommand() { Id = id};
            GenericServiceResponse<DeleteOrderDetailCommandResponse> response = await Mediator.Send(command);
            return Ok(response);
        }
        [HttpGet("GetAllOrderDetails")]
        public async Task<IActionResult> GetAllOrderDetails([FromBody] PageRequest request)
        {
            LoggingService.LogInformation("OrderDetailController get method GetAllOrderDetails called!");
            GetAllOrderDetailsQuery query = new GetAllOrderDetailsQuery() { PageRequest = request};
            GetListResponse<GetAllOrderDetailResponse> response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("GetOrderDetailAllByOrderId")]
        public async Task<IActionResult> GetOrderDetailAllByOrderId([FromBody] PageRequest request)
        {
            LoggingService.LogInformation("OrderDetailController get method GetOrderDetailAllByOrderId called!");
            GetOrderDetailAllByOrderIdQuery query = new GetOrderDetailAllByOrderIdQuery() { PageRequest = request};
            GetListResponse<GetAllOrderDetailResponse> response = await Mediator.Send(query);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById([FromRoute] Guid id)
        {
            LoggingService.LogInformation("OrderDetailController get method GetOrderDetailById called!");
            GetOrderDetailByIdQuery query = new GetOrderDetailByIdQuery() { orderDetailId = id};
            GenericServiceResponse<GetOrderDetailByIdResponse> response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("GetOrderDetailByOrderId/{id}")]
        public async Task<IActionResult> GetOrderDetailByOrderId([FromRoute] Guid id)
        {
            LoggingService.LogInformation("OrderDetailController get method GetOrderDetailByOrderId/id called!");
            GetOrderDetailByOrderIdQuery query = new GetOrderDetailByOrderIdQuery() { OrderId = id};
            GenericServiceResponse<GetOrderDetailByIdResponse> response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpPut("UpdatedOrderDetail")]
        public async Task<IActionResult> UpdatedOrderDetail([FromBody] UpdatedOrderDetailCommand command)
        {
            LoggingService.LogInformation("OrderDetailController update method UpdatedOrderDetail called!");
            GenericServiceResponse<UpdatedOrderDetailCommandResponse> response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
