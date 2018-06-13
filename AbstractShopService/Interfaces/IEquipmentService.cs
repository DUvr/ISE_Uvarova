using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IEquipmentService
    {
        List<EquipmentViewModel> GetList();

        EquipmentViewModel GetElement(int id);

        void AddElement(EquipmentBM model);

        void UpdElement(EquipmentBM model);

        void DelElement(int id);
    }
}
