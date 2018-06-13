using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<ZakazchikViewModel> result = source.Clients
                .Select(rec => new ZakazchikViewModel
                {
                    Id = rec.Id,
                    ClientFIO = rec.ZakazchikFIO
                })
                .ToList();
            return result;
        }

        public ZakazchikViewModel GetElement(int id)
        {
            Zakazchik element = source.Clients.FirstOrDefault(rec => rec.Id == id);
            if(element != null)
            {
                return new ZakazchikViewModel
                {
                    Id = element.Id,
                    ClientFIO = element.ZakazchikFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ZakazchikBindingModel model)
        {
            Zakazchik element = source.Clients.FirstOrDefault(rec => rec.ZakazchikFIO == model.ClientFIO);
            if(element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            int maxId = source.Clients.Count > 0 ? source.Clients.Max(rec => rec.Id) : 0;
            source.Clients.Add(new Zakazchik
            {
                Id = maxId + 1,
                ZakazchikFIO = model.ClientFIO
            });
        }

        public void UpdElement(ZakazchikBindingModel model)
        {
            Zakazchik element = source.Clients.FirstOrDefault(rec => 
                                    rec.ZakazchikFIO == model.ClientFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = source.Clients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ZakazchikFIO = model.ClientFIO;
        }

        public void DelElement(int id)
        {
            Zakazchik element = source.Clients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Clients.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
