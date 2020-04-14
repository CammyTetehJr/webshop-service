using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace csa_week_3_webshop
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract (Namespace = "service",CallbackContract = typeof(IWebShopCallBack))]
    public interface IWebShop
    {
        // TODO: Add your service operations here
        [OperationContract]
        string GetWebShopName();
        [OperationContract]
        List<Product> GetProductList();
        [OperationContract]
        String GetProductInfo(string name);
        [OperationContract (IsOneWay =true)]
        void BuyProduct(string name);
        [OperationContract (IsOneWay = true)]
        void Subscribe();
        [OperationContract]
        void UnSubscribe();


    }

    [ServiceContract(Namespace = "service", CallbackContract = typeof(IShippingCallBack))]

    public interface IShipping
    {

        [OperationContract]
        List<Order> GetOrderList();
        [OperationContract]
        bool ShipOrder(int OrderId);
        [OperationContract]
        void SubscribeEvent();
        [OperationContract]
        void UnSubscribeEvent();

    }




    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class WebShop
    {
        string webshopName = "Cammy's webshop";
        List<Product> products;



        [DataMember]
        public String WebShopName
        {
            get { return webshopName; }
            set { webshopName = value; }

        }

        [DataMember]
        public List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }

        public WebShop()
        {
            
        }



    }

    [DataContract]
    public class Product
    {
        private string name;
        private string description;
        private decimal price;
        private double profitMargin;
        private int stock;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        [DataMember]
        public Decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public double ProfitMargin
        {
            get { return profitMargin; }
            set { profitMargin = value; }
        }
        [DataMember]
        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }

        public Product(string name, string description, decimal price, int stock, double profit)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.stock = stock;
            this.profitMargin = profit;

        }


    }

    [DataContract]
    public class Order
    {
        private static int Count;
        [DataMember]
        public int OrderId { get; set; }
        [DataMember]
        public string ProductId { get; set; }
        [DataMember]
        public DateTime Moment { get; set; }
        public IWebShopCallBack WebShopCallBack { get; set; }

        public Order(string productId,IWebShopCallBack callback)
        {
            OrderId = Count++;
            ProductId = productId;
            Moment = DateTime.Now;
            WebShopCallBack = callback;
        }
    }
}
