using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IAdministrantService
    {
        List<AdministrantViewModel> GetList();

        AdministrantViewModel GetElement(int id);

        void AddElement(AdministrantBM model);

        void UpdElement(AdministrantBM model);

        void DelElement(int id);
    }
}
