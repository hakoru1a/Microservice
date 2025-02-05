using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constracts.Saga.OrderManager
{
    public interface ISagaOrderManager<in Tinput, out Toutput> where Tinput : class where Toutput : class
    {
        Toutput CreateOrder(Tinput Tinput);

        Toutput RollbackOrder(string username, string documentNo, long orderId);
    }
}
