using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class ClientServiceList : IZakazchikService
    {
        private DataListSingleton source;

        public ClientServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ZakazchikViewModel> GetList()
        {
            List<ZakazchikViewModel> result = new List<ZakazchikViewModel>();
            for (int i = 0; i < source.Zakazchik.Count; ++i)
            {
                result.Add(new ZakazchikViewModel
                {
                    Id = source.Zakazchik[i].Id,
                    ClientFIO = source.Zakazchik[i].ClientFIO
                });
            }
            return result;
        }

        public ZakazchikViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Zakazchik.Count; ++i)
            {
                if (source.Zakazchik[i].Id == id)
                {
                    return new ZakazchikViewModel
                    {
                        Id = source.Zakazchik[i].Id,
                        ClientFIO = source.Zakazchik[i].ClientFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ZakazchikBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Zakazchik.Count; ++i)
            {
                if (source.Zakazchik[i].Id > maxId)
                {
                    maxId = source.Zakazchik[i].Id;
                }
                if (source.Zakazchik[i].ClientFIO == model.ZakazchikFIO)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            source.Zakazchik.Add(new Zakazchik {
                Id = maxId + 1,
                ClientFIO = model.ZakazchikFIO
            });
        }

        public void UpdElement(ZakazchikBM model)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazchik.Count; ++i)
            {
                if (source.Zakazchik[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Zakazchik[i].ClientFIO == model.ZakazchikFIO && 
                    source.Zakazchik[i].Id != model.Id)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Zakazchik[index].ClientFIO = model.ZakazchikFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Zakazchik.Count; ++i)
            {
                if (source.Zakazchik[i].Id == id)
                {
                    source.Zakazchik.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
