﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Basket
{
    public class CartItemDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string No { get; set; }
        public string Name { get; set; }

        public int AvailableStock { get; set; }

        public string? ExternalDocumentNo { get; set; }

    }
}
