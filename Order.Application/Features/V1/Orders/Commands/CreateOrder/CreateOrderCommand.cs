using AutoMapper;
using EventBus.Messages.IntergrationEvent.Event;
using MediatR;
using Order.Application.Common.Mappings;
using Order.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderCatalog = Order.Domain.Entities.Order;
namespace Order.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<ApiResult<long>>, IMapFrom<OrderCatalog>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public string Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrderCommand, OrderCatalog>();

            profile.CreateMap<BasketCheckoutEvent, CreateOrderCommand>();
        }
    }
}
