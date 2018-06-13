using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IBasicSecurityEquipmentService
    {
        List<BasicSecurityEquipmentViewModel> GetList();

        BasicSecurityEquipmentViewModel GetElement(int id);

        void AddElement(BasicSecurityEquipmentBM model);

        void UpdElement(BasicSecurityEquipmentBM model);

        void DelElement(int id);
    }
}
