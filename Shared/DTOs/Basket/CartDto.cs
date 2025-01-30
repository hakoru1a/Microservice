using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Basket
{
        public class CartDto
        {
            public CartDto()
            {
            }

            public CartDto(string username)
            {
                Username = username;
            }

            public string Username { get; set; }

            public string Email { get; set; }
            public List<CartItemDto> Items { get; set; } = new();

            public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

        }

}
