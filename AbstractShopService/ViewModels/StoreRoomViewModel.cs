using System.Collections.Generic;

namespace AbstractShopService.ViewModels
{
    public class StoreRoomViewModel
    {
        public int Id { get; set; }

        public string StockName { get; set; }

        public List<StoreRoomEquipmentsViewModel> StockComponents { get; set; }
    }
}
