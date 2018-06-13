using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IStoreRoomService
    {
        List<StoreRoomViewModel> GetList();

        StoreRoomViewModel GetElement(int id);

        void AddElement(StoreRoomBM model);

        void UpdElement(StoreRoomBM model);

        void DelElement(int id);
    }
}
