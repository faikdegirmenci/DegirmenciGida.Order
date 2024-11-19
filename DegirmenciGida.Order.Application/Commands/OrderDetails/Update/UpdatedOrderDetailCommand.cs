
using Application;
using AutoMapper;
using DegirmenciGida.Order.Application.Commands.OrderDetails.Update;
using MediatR;

namespace DegirmenciGida.Order.Application
{
    public class UpdatedOrderDetailCommand:IRequest<GenericServiceResponse<UpdatedOrderDetailCommandResponse>>
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public class UpdatedOrderDetailCommandHandler:IRequestHandler<UpdatedOrderDetailCommand,GenericServiceResponse<UpdatedOrderDetailCommandResponse>>
        {
            private readonly IOrderDetailService _service;
            private readonly IMapper _mapper;

            public UpdatedOrderDetailCommandHandler(IOrderDetailService service, IMapper mapper)
            {
                _service = service;
                _mapper = mapper;
            }

            public async Task<GenericServiceResponse<UpdatedOrderDetailCommandResponse>> Handle(UpdatedOrderDetailCommand request, CancellationToken cancellationToken)
            {
                GenericServiceResponse<UpdatedOrderDetailCommandResponse> response = new GenericServiceResponse<UpdatedOrderDetailCommandResponse>();

                try
                {                   
                    var orderDetail = await _service.GetAsync(predicate:o=>o.Id == request.Id,cancellationToken:cancellationToken);
                    orderDetail = _mapper.Map(request, orderDetail);
                    orderDetail.UpdatedDate = DateTime.Now;

                    await _service.UpdateAsync(orderDetail);
                    response.Data = _mapper.Map<UpdatedOrderDetailCommandResponse>(orderDetail);
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Errors.Add(ex.Message);
                    return response;
                }

                response.Success = true;
                response.Message = "OK";
                return response;
            }
        }
    }
}
