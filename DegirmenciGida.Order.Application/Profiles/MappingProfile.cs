using Application;
using Application.Response;
using AutoMapper;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Commands.Order.Delete;
using DegirmenciGida.Order.Application.Commands.Order.Update;
using DegirmenciGida.Order.Application.Commands.OrderDetails.Delete;
using DegirmenciGida.Order.Application.Commands.OrderDetails.Update;
using DegirmenciGida.Order.Application.Queries.Order.GetById;
using DegirmenciGida.Order.Application.Queries.Order.GetList;
using DegirmenciGida.Order.Application.Queries.OrderDetails.GetById;
using DegirmenciGida.Order.Application.Queries.OrderDetails.GetList;
using DegirmenciGida.Order.Domain;
using Persistence.Paging;

namespace DegirmenciGida.Order.Application.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Orders,CreateOrderCommandResponse>().ReverseMap();
            CreateMap<Orders,GenericServiceResponse<CreateOrderCommandResponse>>().ReverseMap();


            CreateMap<Orders, DeleteOrderCommandResponse>().ReverseMap();
            CreateMap<Orders, GenericServiceResponse<DeleteOrderCommandResponse>>().ReverseMap();


            CreateMap<Orders, GetOrderByIdResponse>().ReverseMap();
            CreateMap<Orders, GenericServiceResponse<GetOrderByIdResponse>>().ReverseMap();


            CreateMap<Orders, GetAllOrderResponse>().ReverseMap();
            CreateMap<Paginate<Orders>, GetAllOrderResponse>().ReverseMap();
            CreateMap<Paginate<Orders>, GetListResponse<GetAllOrderResponse>>().ReverseMap();

            CreateMap<OrderDetail, DeleteOrderDetailCommandResponse>().ReverseMap();
            CreateMap<OrderDetail,GenericServiceResponse<DeleteOrderDetailCommandResponse>>().ReverseMap();

            CreateMap<OrderDetail, UpdatedOrderDetailCommand>().ReverseMap();
            CreateMap<OrderDetail,UpdatedOrderDetailCommandResponse>().ReverseMap();
            CreateMap<OrderDetail,GenericServiceResponse<UpdatedOrderDetailCommandResponse>>().ReverseMap();

            CreateMap<OrderDetail,GetAllOrderDetailResponse>().ReverseMap();
            CreateMap<Paginate<OrderDetail>,GetAllOrderDetailResponse>().ReverseMap();
            CreateMap<Paginate<OrderDetail>,GetListResponse<GetAllOrderDetailResponse>>().ReverseMap();

            CreateMap<OrderDetail, GetOrderDetailByIdResponse>().ReverseMap();
            CreateMap<OrderDetail,GenericServiceResponse<GetOrderDetailByIdResponse>>().ReverseMap();
        }
    }
}
