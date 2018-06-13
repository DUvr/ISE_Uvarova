using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class StockServiceList : IStoreRoomService
    {
        private DataListSingleton source;

        public StockServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<StoreRoomViewModel> GetList()
        {
            List<StoreRoomViewModel> result = new List<StoreRoomViewModel>();
            for (int i = 0; i < source.StoreRoom.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StoreRoomEquipmentsViewModel> StockComponents = new List<StoreRoomEquipmentsViewModel>();
                for (int j = 0; j < source.StoreRoomEquipments.Count; ++j)
                {
                    if (source.StoreRoomEquipments[j].StockId == source.StoreRoom[i].Id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Equipment.Count; ++k)
                        {
                            if (source.Equipment_BSEquipment[j].ComponentId == source.Equipment[k].Id)
                            {
                                componentName = source.Equipment[k].ComponentName;
                                break;
                            }
                        }
                        StockComponents.Add(new StoreRoomEquipmentsViewModel
                        {
                            Id = source.StoreRoomEquipments[j].Id,
                            StockId = source.StoreRoomEquipments[j].StockId,
                            ComponentId = source.StoreRoomEquipments[j].ComponentId,
                            ComponentName = componentName,
                            Count = source.StoreRoomEquipments[j].Count
                        });
                    }
                }
                result.Add(new StoreRoomViewModel
                {
                    Id = source.StoreRoom[i].Id,
                    StockName = source.StoreRoom[i].StockName,
                    StockComponents = StockComponents
                });
            }
            return result;
        }

        public StoreRoomViewModel GetElement(int id)
        {
            for (int i = 0; i < source.StoreRoom.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StoreRoomEquipmentsViewModel> StockComponents = new List<StoreRoomEquipmentsViewModel>();
                for (int j = 0; j < source.StoreRoomEquipments.Count; ++j)
                {
                    if (source.StoreRoomEquipments[j].StockId == source.StoreRoom[i].Id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Equipment.Count; ++k)
                        {
                            if (source.Equipment_BSEquipment[j].ComponentId == source.Equipment[k].Id)
                            {
                                componentName = source.Equipment[k].ComponentName;
                                break;
                            }
                        }
                        StockComponents.Add(new StoreRoomEquipmentsViewModel
                        {
                            Id = source.StoreRoomEquipments[j].Id,
                            StockId = source.StoreRoomEquipments[j].StockId,
                            ComponentId = source.StoreRoomEquipments[j].ComponentId,
                            ComponentName = componentName,
                            Count = source.StoreRoomEquipments[j].Count
                        });
                    }
                }
                if (source.StoreRoom[i].Id == id)
                {
                    return new StoreRoomViewModel
                    {
                        Id = source.StoreRoom[i].Id,
                        StockName = source.StoreRoom[i].StockName,
                        StockComponents = StockComponents
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StoreRoomBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.StoreRoom.Count; ++i)
            {
                if (source.StoreRoom[i].Id > maxId)
                {
                    maxId = source.StoreRoom[i].Id;
                }
                if (source.StoreRoom[i].StockName == model.StockName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.StoreRoom.Add(new StoreRoom
            {
                Id = maxId + 1,
                StockName = model.StockName
            });
        }

        public void UpdElement(StoreRoomBM model)
        {
            int index = -1;
            for (int i = 0; i < source.StoreRoom.Count; ++i)
            {
                if (source.StoreRoom[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.StoreRoom[i].StockName == model.StockName && 
                    source.StoreRoom[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.StoreRoom[index].StockName = model.StockName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.StoreRoomEquipments.Count; ++i)
            {
                if (source.StoreRoomEquipments[i].StockId == id)
                {
                    source.StoreRoomEquipments.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.StoreRoom.Count; ++i)
            {
                if (source.StoreRoom[i].Id == id)
                {
                    source.StoreRoom.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
