using CarpetCleaningSystem.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Orders.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }
    }
}
