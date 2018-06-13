using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IZakazchikService
    {
        List<ZakazchikViewModel> GetList();

        ZakazchikViewModel GetElement(int id);

        void AddElement(ZakazchikBM model);

        void UpdElement(ZakazchikBM model);

        void DelElement(int id);
    }
}
