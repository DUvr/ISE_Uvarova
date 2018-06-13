using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class ComponentServiceList : IEquipmentService
    {
        private DataListSingleton source;

        public ComponentServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<EquipmentViewModel> GetList()
        {
            List<EquipmentViewModel> result = source.Components
                .Select(rec => new EquipmentViewModel
                {
                    Id = rec.Id,
                    ComponentName = rec.ComponentName
                })
                .ToList();
            return result;
        }

        public EquipmentViewModel GetElement(int id)
        {
            Equipment element = source.Components.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new EquipmentViewModel
                {
                    Id = element.Id,
                    ComponentName = element.ComponentName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(EquipmentBindingModel model)
        {
            Equipment element = source.Components.FirstOrDefault(rec => rec.ComponentName == model.ComponentName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.Components.Count > 0 ? source.Components.Max(rec => rec.Id) : 0;
            source.Components.Add(new Equipment
            {
                Id = maxId + 1,
                ComponentName = model.ComponentName
            });
        }

        public void UpdElement(EquipmentBindingModel model)
        {
            Equipment element = source.Components.FirstOrDefault(rec => 
                                        rec.ComponentName == model.ComponentName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Components.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ComponentName = model.ComponentName;
        }

        public void DelElement(int id)
        {
            Equipment element = source.Components.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Components.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
