using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class ImplementerServiceList : IAdministrantService
    {
        private DataListSingleton source;

        public ImplementerServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<AdministrantViewModel> GetList()
        {
            List<AdministrantViewModel> result = new List<AdministrantViewModel>();
            for (int i = 0; i < source.Administrant.Count; ++i)
            {
                result.Add(new AdministrantViewModel
                {
                    Id = source.Administrant[i].Id,
                    AdministrantFIO = source.Administrant[i].AdministrantFIO
                });
            }
            return result;
        }

        public AdministrantViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Administrant.Count; ++i)
            {
                if (source.Administrant[i].Id == id)
                {
                    return new AdministrantViewModel
                    {
                        Id = source.Administrant[i].Id,
                        AdministrantFIO = source.Administrant[i].AdministrantFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(AdministrantBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Administrant.Count; ++i)
            {
                if (source.Administrant[i].Id > maxId)
                {
                    maxId = source.Administrant[i].Id;
                }
                if (source.Administrant[i].AdministrantFIO == model.AdministrantFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Administrant.Add(new Administrant
            {
                Id = maxId + 1,
                AdministrantFIO = model.AdministrantFIO
            });
        }

        public void UpdElement(AdministrantBM model)
        {
            int index = -1;
            for (int i = 0; i < source.Administrant.Count; ++i)
            {
                if (source.Administrant[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Administrant[i].AdministrantFIO == model.AdministrantFIO && 
                    source.Administrant[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Administrant[index].AdministrantFIO = model.AdministrantFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Administrant.Count; ++i)
            {
                if (source.Administrant[i].Id == id)
                {
                    source.Administrant.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
