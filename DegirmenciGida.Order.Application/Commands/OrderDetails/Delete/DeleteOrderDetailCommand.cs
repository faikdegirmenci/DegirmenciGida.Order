using Application;
using AutoMapper;
using MediatR;

namespace DegirmenciGida.Order.Application.Commands.OrderDetails.Delete
{
    public class DeleteOrderDetailCommand:IRequest<GenericServiceResponse<DeleteOrderDetailCommandResponse>>
    {
        public Guid Id { get; set; }

        public class DeleteOrderDetailCommandHandler : IRequestHandler<DeleteOrderDetailCommand, GenericServiceResponse<DeleteOrderDetailCommandResponse>>
        {
            private readonly IOrderDetailService _service;
            private readonly IMapper _mapper;

            public DeleteOrderDetailCommandHandler(IOrderDetailService service, IMapper mapper)
            {
                _service = service;
                _mapper = mapper;
            }

            public async Task<GenericServiceResponse<DeleteOrderDetailCommandResponse>> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
            {
                GenericServiceResponse<DeleteOrderDetailCommandResponse> response = new GenericServiceResponse<DeleteOrderDetailCommandResponse>();
                try
                {
                    var orderDetail = await _service.GetAsync(predicate:o=>o.Id == request.Id,cancellationToken:cancellationToken);
                    await _service.DeleteAsync(orderDetail);
                    response.Success = true;
                    response.Message = "OK";
                    response.Data = _mapper.Map<DeleteOrderDetailCommandResponse>(orderDetail);
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Errors.Add(ex.Message);
                    return response;
                }


                return response;
            }
        }
    }
}
