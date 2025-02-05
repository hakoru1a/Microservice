using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class CreatedSalesOrderSuccessDto
    {
        public string DocumentNo { get; set; }

        public CreatedSalesOrderSuccessDto(string DocumentNo)
        {
            this.DocumentNo = DocumentNo;
        }
    }
}
