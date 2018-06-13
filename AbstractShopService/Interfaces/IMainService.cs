using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IMainService
    {
        List<SafetySystemViewModel> GetList();

        void CreateOrder(SafetySystemBM model);

        void TakeOrderInWork(SafetySystemBM model);

        void FinishOrder(int id);

        void PayOrder(int id);

        void PutComponentOnStock(StoreRoomEquipmentsBM model);
    }
}
