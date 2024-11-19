
using Application;
using AutoMapper;
using DegirmenciGida.Order.Domain;
using MediatR;

namespace DegirmenciGida.Order.Application.Queries.OrderDetails.GetById
{
    public class GetOrderDetailByOrderIdQuery:IRequest<GenericServiceResponse<GetOrderDetailByIdResponse>>
    {
        public Guid OrderId { get; set; }

        public class GetOrderDetailByOrderIdQueryHandler : IRequestHandler<GetOrderDetailByOrderIdQuery, GenericServiceResponse<GetOrderDetailByIdResponse>>
        {
            private readonly IOrderDetailService _orderDetailService;
            private readonly IMapper _mapper;

            public GetOrderDetailByOrderIdQueryHandler(IOrderDetailService orderDetailService, IMapper mapper)
            {
                _orderDetailService = orderDetailService;
                _mapper = mapper;
            }

            public async Task<GenericServiceResponse<GetOrderDetailByIdResponse>> Handle(GetOrderDetailByOrderIdQuery request, CancellationToken cancellationToken)
            {
                GenericServiceResponse<GetOrderDetailByIdResponse> response = new GenericServiceResponse<GetOrderDetailByIdResponse>();
                try
                {
                    var orderDetail =  await _orderDetailService.GetAsync(predicate:o=>o.OrdersId == request.OrderId,cancellationToken:cancellationToken);
                    response.Data = _mapper.Map<GetOrderDetailByIdResponse>(orderDetail);
                    response.Success = true;
                    response.Message = "OK";
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
