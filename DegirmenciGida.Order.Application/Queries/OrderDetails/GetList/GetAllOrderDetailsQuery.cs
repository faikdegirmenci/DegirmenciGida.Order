
using Application;
using Application.Response;
using AutoMapper;
using DegirmenciGida.Order.Domain;
using MediatR;
using Persistence.Paging;

namespace DegirmenciGida.Order.Application.Queries.OrderDetails.GetList
{
    public class GetAllOrderDetailsQuery:IRequest<GetListResponse<GetAllOrderDetailResponse>>
    {
        public PageRequest PageRequest { get; set; }
        public class GetAllOrderDetailsQueryHandler:IRequestHandler<GetAllOrderDetailsQuery, GetListResponse<GetAllOrderDetailResponse>>
        {
            private readonly IOrderDetailService _orderDetailService;
            private readonly IMapper _mapper;

            public GetAllOrderDetailsQueryHandler(IOrderDetailService orderDetailService, IMapper mapper)
            {
                _orderDetailService = orderDetailService;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetAllOrderDetailResponse>> Handle(GetAllOrderDetailsQuery request, CancellationToken cancellationToken)
            {
                Paginate<OrderDetail> orderDetail = await _orderDetailService.GetListAsync(
                                                 index: request.PageRequest.PageIndex,
                                                 size: request.PageRequest.PageSize,
                                                 cancellationToken: cancellationToken);

                GetListResponse<GetAllOrderDetailResponse> response = _mapper.Map<GetListResponse<GetAllOrderDetailResponse>>(orderDetail);
                return response;
            }
        }
    }
}
