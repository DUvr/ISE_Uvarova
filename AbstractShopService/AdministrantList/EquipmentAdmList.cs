using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<EquipmentViewModel> result = new List<EquipmentViewModel>();
            for (int i = 0; i < source.Equipment.Count; ++i)
            {
                result.Add(new EquipmentViewModel
                {
                    Id = source.Equipment[i].Id,
                    ComponentName = source.Equipment[i].ComponentName
                });
            }
            return result;
        }

        public EquipmentViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Equipment.Count; ++i)
            {
                if (source.Equipment[i].Id == id)
                {
                    return new EquipmentViewModel
                    {
                        Id = source.Equipment[i].Id,
                        ComponentName = source.Equipment[i].ComponentName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(EquipmentBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Equipment.Count; ++i)
            {
                if (source.Equipment[i].Id > maxId)
                {
                    maxId = source.Equipment[i].Id;
                }
                if (source.Equipment[i].ComponentName == model.ComponentName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Equipment.Add(new Equipment
            {
                Id = maxId + 1,
                ComponentName = model.ComponentName
            });
        }

        public void UpdElement(EquipmentBM model)
        {
            int index = -1;
            for (int i = 0; i < source.Equipment.Count; ++i)
            {
                if (source.Equipment[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Equipment[i].ComponentName == model.ComponentName && 
                    source.Equipment[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Equipment[index].ComponentName = model.ComponentName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Equipment.Count; ++i)
            {
                if (source.Equipment[i].Id == id)
                {
                    source.Equipment.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
