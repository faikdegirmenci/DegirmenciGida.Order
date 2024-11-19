
using Application;
using AutoMapper;
using DegirmenciGida.Order.Application.Queries.Order.GetById;
using DegirmenciGida.Order.Domain;
using MediatR;

namespace DegirmenciGida.Order.Application.Queries.Order.GetById
{
    public class GetOrderByIdQuery:IRequest<GenericServiceResponse<GetOrderByIdResponse>>
    {
        public Guid Id { get; set; }

        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GenericServiceResponse<GetOrderByIdResponse>>
        {
            private readonly IOrderService _orderService;
            private readonly IMapper _mapper;

            public GetOrderByIdQueryHandler(IOrderService orderService, IMapper mapper)
            {
                _orderService = orderService;
                _mapper = mapper;
            }

            public async Task<GenericServiceResponse<GetOrderByIdResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
                GenericServiceResponse<GetOrderByIdResponse> genericServiceResponse = new GenericServiceResponse<GetOrderByIdResponse>();

                try
                {
                    var order = await _orderService.GetAsync(predicate:o=>o.Id == request.Id,cancellationToken:cancellationToken);
                    genericServiceResponse.Data = _mapper.Map<GetOrderByIdResponse>(order);
                }
                catch (Exception ex)
                {
                    genericServiceResponse.Success = false;
                    genericServiceResponse.Errors.Add(ex.Message);
                    return genericServiceResponse;
                }

                genericServiceResponse.Success = true;
                genericServiceResponse.Message = "Get order successfull!";
                return genericServiceResponse;
            }
        }
    }
}
