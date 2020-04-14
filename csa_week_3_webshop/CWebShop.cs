using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace csa_week_3_webshop
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class CWebShop : IWebShop,IShipping
    {

        private WebShop webshop;
        private List<Product> products;
        public List<Order> orders;
        private Action<Order> productSoldEvent = delegate { };


        static List<IWebShopCallBack> m_Callbacks = new List<IWebShopCallBack>();
        static List<IShippingCallBack> s_CallBacks = new List<IShippingCallBack>();

        public void Subscribe()
        {
            IWebShopCallBack callback = OperationContext.Current.GetCallbackChannel<IWebShopCallBack>();
            if (m_Callbacks.Contains(callback) == false)
            {
                m_Callbacks.Add(callback);
                ClientCount();
            }
        }

        public void UnSubscribe()
        {
            IWebShopCallBack callback = OperationContext.Current.GetCallbackChannel<IWebShopCallBack>();
            if (m_Callbacks.Contains(callback))
            {
                m_Callbacks.Remove(callback);
            }
            else
            {
                throw new InvalidOperationException("Cannot find callback");
            }
        }

        public static void ClientCount()
        {


                foreach (IWebShopCallBack callback in m_Callbacks)
                {
                    callback.NewClientConnected(m_Callbacks.Count);
                
                }
        }

        public static void ClientProductSold(Product product)
        {
            foreach (IWebShopCallBack callback in m_Callbacks)
            {
                callback.ProductSold(product);

            }
        }

        
 

        public Product GetProduct(string name)
        {
            Product p = null;
            foreach (Product product in products)
            {
                if (product.Name == name)
                {
                    p = product;

                }
     
            }
            return p;
        }
        
        public void BuyProduct(string name)
        {
           Product p =  GetProduct(name);
            //bool status = false;
            if(p != null && p.Stock > 0)
            {
                p.Stock--;
                ClientProductSold(p);
                IWebShopCallBack callback = OperationContext.Current.GetCallbackChannel<IWebShopCallBack>();
                Order temp = new Order(p.Name, callback);
                orders.Add(temp);
                productSoldEvent(temp);
                

                //status = true;
            }
            //ClientProductSold(p);
            //return status;
        }

        public string GetProductInfo(string name)
        {
            Product p = null;
            foreach (Product product in products)
            {
                if (product.Name == name)
                {
                    p = product;
                }
            }
            if (p != null)
            {
                return "Name : " + p.Name + " price: " + p.Price + " stock: " + p.Stock;
            }
            else
            {
                return "no product with that name";
            }
        }

        public List<Product> GetProductList()
        {
            return products;
        }

        public string GetWebShopName()
        {
            return webshop.WebShopName;
        }

        public List<Order> GetOrderList()
        {
            return orders;
        }

        public bool ShipOrder(int OrderId)
        {
            return true;
        }

        public void SubscribeEvent()
        {
            IShippingCallBack callback = OperationContext.Current.GetCallbackChannel<IShippingCallBack>();
            if (s_CallBacks.Contains(callback) == false)
            {
                s_CallBacks.Add(callback);
                productSoldEvent += callback.OnProductBought;

            }


        }

        public void UnSubscribeEvent()
        {
            IShippingCallBack callback = OperationContext.Current.GetCallbackChannel<IShippingCallBack>();
            if (s_CallBacks.Contains(callback) == true)
            {
                s_CallBacks.Remove(callback);
                productSoldEvent -= callback.OnProductBought;
            }

        }

        public CWebShop()
        {
            webshop = new WebShop();
            
            
            products = new List<Product>();
            orders = new List<Order>();

            products.Add(new Product("Dracula", "Horror book", 40.00m, 20, 140.00));
            products.Add(new Product("Hello", "Comic book", 80.00m, 10, 100.00));
            products.Add(new Product("Magneto", "Scifi book", 10.00m, 20, 140.00));
            products.Add(new Product("Den", "cook book", 100.00m, 20, 140.00));
            products.Add(new Product("Ovation", "fashion book", 90.00m, 20, 10.00));
            products.Add(new Product("Pop culture", "music book", 20.00m, 20, 15.00));
            products.Add(new Product("Investment", "finance book", 10.00m, 20, 320.00));
            products.Add(new Product("Kidney check", "health book", 48.00m, 20, 15.00));
            products.Add(new Product("Furter Maths", "math book", 490.00m, 20, 40.00));
            products.Add(new Product("CSA", "csa book", 640.00m, 20, 1140.00));

        }

    }
}
