using Application;
using AutoMapper;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Domain;
using MediatR;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DegirmenciGida.Order.Application
{
    public class CreateOrderRabbitMqCommand : IRequest<GenericServiceResponse<CreateOrderCommandResponse>>
    {
        public List<OrderRequest> OrderRequest { get; set; }
        public Guid CustomerId { get; set; }
        public class CreateOrderRabbitMqCommandHandler:IRequestHandler<CreateOrderRabbitMqCommand,GenericServiceResponse<CreateOrderCommandResponse>> 
        {
            private readonly IOrderService _orderService;
            private readonly IOrderDetailService _orderDetailService;
            private readonly IMapper _mapper;
            private readonly IOrderRabbitMQService _rabbitMQService;

            public CreateOrderRabbitMqCommandHandler(IOrderService orderService, IOrderDetailService orderDetailService, IMapper mapper, IOrderRabbitMQService rabbitMQService)
            {
                _orderService = orderService;
                _orderDetailService = orderDetailService;
                _mapper = mapper;
                _rabbitMQService = rabbitMQService;
            }

            public async Task<GenericServiceResponse<CreateOrderCommandResponse>> Handle(CreateOrderRabbitMqCommand request, CancellationToken cancellationToken)
            {
                GenericServiceResponse<CreateOrderCommandResponse> genericServiceResponse = new GenericServiceResponse<CreateOrderCommandResponse>();
                try
                {
                    decimal totalAmount = 0;
                    Orders orders = new Orders(request.CustomerId);

                    if (request.OrderRequest != null && request.OrderRequest.Count > 0)
                    {
                        orders.CreatedDate = DateTime.Now;
                        orders.IsDeleted = false;

                        orders = await _orderService.AddAsync(orders);

                        orders.OrderDetails = new List<OrderDetail>();


                        foreach (var item in request.OrderRequest)
                        {
                            var product = _rabbitMQService.CreateOrderAsync(item.ProductId);

                            if (product != null && !string.IsNullOrEmpty(product.Result.Description) && !string.IsNullOrEmpty(product.Result.Name))
                            {
                                OrderDetail orderDetail = new OrderDetail();

                                orderDetail.ProductId = product.Result.Id;
                                orderDetail.ProductName = product.Result.Name;
                                orderDetail.ProductPrice = product.Result.Price;
                                orderDetail.Quantity = item.Quantity;
                                orderDetail.CreatedDate = DateTime.Now;
                                orderDetail.IsDeleted = false;
                                orderDetail.OrdersId = orders.Id;
                                totalAmount += product.Result.Price * item.Quantity;

                                var detailOrder = await _orderDetailService.AddAsync(orderDetail);
                                orders.OrderDetails.Add(detailOrder);
                            }
                            else
                            {
                                genericServiceResponse.Success = false;
                                genericServiceResponse.Errors.Add("Get Product failed!");
                                return genericServiceResponse;
                            }
                        }
                        orders.TotalAmount = totalAmount;
                        await _orderService.UpdateAsync(orders);
                    }

                    genericServiceResponse.Success = true;
                    genericServiceResponse.Message = "Create Order successfull!";
                    genericServiceResponse.Data = _mapper.Map<CreateOrderCommandResponse>(orders);
                }
                catch (Exception ex)
                {
                    genericServiceResponse.Success = false;
                    genericServiceResponse.Errors.Add(ex.Message);
                    return genericServiceResponse;
                }

                return genericServiceResponse;
            }

        }
    }
}
