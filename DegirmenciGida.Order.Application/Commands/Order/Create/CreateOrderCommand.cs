
using Application;
using AutoMapper;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Interfaces;
using DegirmenciGida.Order.Domain;
using Infrastructure.LoggingServices;
using MediatR;
using Persistence.Saga;
using System.Text.Json;

namespace DegirmenciGida.Order.Application.Commands.Order.Create
{
    public class CreateOrderCommand : IRequest<GenericServiceResponse<CreateOrderCommandResponse>>
    {
        public List<OrderRequest> OrderRequest { get; set; }
        public Guid CustomerId { get; set; }

        public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand, GenericServiceResponse<CreateOrderCommandResponse>> 
        {
            private readonly IOrderProcessService _orderProcessService;

            public CreateOrderCommandHandler(IOrderProcessService orderProcessService)
            {
                _orderProcessService = orderProcessService;
            }

            public async Task<GenericServiceResponse<CreateOrderCommandResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var response = new GenericServiceResponse<CreateOrderCommandResponse>();

                try
                {
                    // Saga Pattern ile işlemleri başlat
                    var result = await _orderProcessService.ProcessOrder(request);

                    if (result)
                    {
                        response.Success = true;
                        response.Message = "Order created successfully!";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to create order due to insufficient stock.";
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Errors.Add(ex.Message);
                }

                return response;
            }
        }
    }
}
