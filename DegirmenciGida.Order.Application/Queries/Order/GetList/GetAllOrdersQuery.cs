
using Application;
using Application.Response;
using AutoMapper;
using DegirmenciGida.Order.Application.Queries.OrderDetails.GetList;
using DegirmenciGida.Order.Domain;
using MediatR;
using Persistence.Paging;

namespace DegirmenciGida.Order.Application.Queries.Order.GetList
{
    public class GetAllOrdersQuery:IRequest<GetListResponse<GetAllOrderResponse>>
    {
        public PageRequest PageRequest { get; set; }
        public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, GetListResponse<GetAllOrderResponse>>
        {
            private readonly IOrderService _orderService;
            private readonly IMapper _mapper;

            public GetAllOrdersQueryHandler(IOrderService orderService, IMapper mapper)
            {
                _orderService = orderService;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetAllOrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
            {
                Paginate<Orders> order = await _orderService.GetListAsync(
                                                               index: request.PageRequest.PageIndex,
                                                               size: request.PageRequest.PageSize,
                                                               cancellationToken: cancellationToken);

                GetListResponse<GetAllOrderResponse> response = _mapper.Map<GetListResponse<GetAllOrderResponse>>(order);
                return response;
            }
        }
    }
}
