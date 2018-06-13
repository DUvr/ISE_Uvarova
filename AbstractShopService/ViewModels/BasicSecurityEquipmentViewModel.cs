using System.Collections.Generic;

namespace AbstractShopService.ViewModels
{
    public class BasicSecurityEquipmentViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public List<Equipment_BSEquipmentViewModel> ProductComponents { get; set; }
    }
}
