﻿using DegirmenciGida.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Queries.Order.GetById
{
    public class GetOrderByIdResponse
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
