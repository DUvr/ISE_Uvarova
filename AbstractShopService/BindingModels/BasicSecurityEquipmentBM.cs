using System.Collections.Generic;

namespace AbstractShopService.BindingModels
{
    public class BasicSecurityEquipmentBM
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public List<Equipment_BSEquipmentBM> ProductComponents { get; set; }
    }
}
