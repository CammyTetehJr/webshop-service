using System.ServiceModel;

namespace csa_week_3_webshop
{
    public interface IWebShopCallBack
    {
        [OperationContract]
        void NewClientConnected(int numberOfConnectedClients);

        [OperationContract]
        void ProductSold(Product product);
    }
}