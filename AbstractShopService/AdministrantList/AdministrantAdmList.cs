using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<AdministrantViewModel> result = source.Administrant
                .Select(rec => new AdministrantViewModel
                {
                    Id = rec.Id,
                    ImplementerFIO = rec.AdministrantFIO
                })
                .ToList();
            return result;

            List<AdministrantViewModel> result1 =
                (from rec in source.Administrant select new AdministrantViewModel

                {
                Id = rec.Id,
                    ImplementerFIO = rec.AdministrantFIO
                })
                .ToList();
            return result1;
        }

        public AdministrantViewModel GetElement(int id)
        {
            Administrant element = source.Administrant.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new AdministrantViewModel
                {
                    Id = element.Id,
                    ImplementerFIO = element.AdministrantFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(AdministrantBindingModel model)
        {
            Administrant element = source.Administrant.FirstOrDefault(rec => rec.AdministrantFIO == model.ImplementerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxId = source.Administrant.Count > 0 ? source.Administrant.Max(rec => rec.Id) : 0;
            source.Administrant.Add(new Administrant
            {
                Id = maxId + 1,
                AdministrantFIO = model.ImplementerFIO
            });
        }

        public void UpdElement(AdministrantBindingModel model)
        {
            Administrant element = source.Administrant.FirstOrDefault(rec =>
                                        rec.AdministrantFIO == model.ImplementerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Administrant.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.AdministrantFIO = model.ImplementerFIO;
        }

        public void DelElement(int id)
        {
            Administrant element = source.Administrant.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Administrant.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
