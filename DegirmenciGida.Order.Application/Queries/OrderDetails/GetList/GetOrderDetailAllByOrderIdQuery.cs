
using Application;
using Application.Response;
using AutoMapper;
using DegirmenciGida.Order.Application.Queries.OrderDetails.GetList;
using DegirmenciGida.Order.Domain;
using MediatR;
using Persistence.Paging;

namespace DegirmenciGida.Order.Application
{
    public class GetOrderDetailAllByOrderIdQuery:IRequest<GetListResponse<GetAllOrderDetailResponse>>
    {
        public Guid OrderId { get; set; }
        public PageRequest PageRequest { get; set; }
        public class GetOrderDetailAllByOrderIdQueryHandler : IRequestHandler<GetOrderDetailAllByOrderIdQuery, GetListResponse<GetAllOrderDetailResponse>>
        {
            private readonly IOrderDetailService _orderDetailService;
            private readonly IMapper _mapper;

            public GetOrderDetailAllByOrderIdQueryHandler(IOrderDetailService orderDetailService, IMapper mapper)
            {
                _orderDetailService = orderDetailService;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetAllOrderDetailResponse>> Handle(GetOrderDetailAllByOrderIdQuery request, CancellationToken cancellationToken)
            {
                Paginate<OrderDetail> orderDetail = await _orderDetailService.GetListAsync(
                                                               predicate:o=>o.OrdersId == request.OrderId,
                                                               index: request.PageRequest.PageIndex,
                                                               size: request.PageRequest.PageSize,
                                                               cancellationToken: cancellationToken);

                GetListResponse<GetAllOrderDetailResponse> response = _mapper.Map<GetListResponse<GetAllOrderDetailResponse>>(orderDetail);
                return response;
            }
        }
    }
}
