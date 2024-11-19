
using Application;
using AutoMapper;
using MediatR;

namespace DegirmenciGida.Order.Application.Commands.Order.Delete
{
    public class DeleteOrderCommand:IRequest<GenericServiceResponse<DeleteOrderCommandResponse>>
    {
        public Guid Id { get; set; }

        public class DeleteOrderCommandHandler:IRequestHandler<DeleteOrderCommand,GenericServiceResponse<DeleteOrderCommandResponse>>
        {
            private readonly IOrderService _orderService;
            private readonly IOrderDetailService _orderDetailService;
            private readonly IMapper _mapper;

            public DeleteOrderCommandHandler(IOrderService orderService, IOrderDetailService orderDetailService, IMapper mapper)
            {
                _orderService = orderService;
                _orderDetailService = orderDetailService;
                _mapper = mapper;
            }

            public async Task<GenericServiceResponse<DeleteOrderCommandResponse>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                GenericServiceResponse<DeleteOrderCommandResponse> response = new GenericServiceResponse<DeleteOrderCommandResponse>();

                try
                {
                    var orderDetail = await _orderDetailService.GetAsync(predicate: o=>o.OrdersId == request.Id,cancellationToken:cancellationToken);
                    await _orderDetailService.DeleteAsync(orderDetail);

                    var order = await _orderService.GetAsync(predicate:p=>p.Id == request.Id,cancellationToken:cancellationToken);
                    await _orderService.DeleteAsync(order);

                    response.Data = _mapper.Map<DeleteOrderCommandResponse>(order);

                }
                catch (Exception ex)
                {

                    response.Success = false;
                    response.Errors.Add(ex.Message);
                    return response;
                }
                response.Success = true;
                response.Message = "Delete order successfull!";
                return response;
            }
        }
    }
}
