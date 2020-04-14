using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace csa_week_3_webshop
{
    public interface IShippingCallBack
    {
        [OperationContract(IsOneWay = true)]
        void OnProductBought(Order o);


    }
}
