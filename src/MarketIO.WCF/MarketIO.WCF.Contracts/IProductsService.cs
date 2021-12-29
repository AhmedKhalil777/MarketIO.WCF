using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace MarketIO.WCF.Contracts
{

        [ServiceContract]
        public interface IProductsService
        {
            [OperationContract]
            List<Product> GetProducts();
        }
    
}
