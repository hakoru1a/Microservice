using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.V1.Orders.Commands.DeleteOrderByDocumentNo
{
    public class DeleteOrderByDocumentNoCommand : IRequest<ApiResult<bool>>
    {
        public string DocumentNo { get; private set; }

        public DeleteOrderByDocumentNoCommand(string documentNo)
        {
            DocumentNo = documentNo;
        }
    }
}
