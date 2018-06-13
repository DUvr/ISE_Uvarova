using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<StoreRoomViewModel> result = source.Stocks
                .Select(rec => new StoreRoomViewModel
                {
                    Id = rec.Id,
                    StockName = rec.StockName,
                    StockComponents = source.StockComponents
                            .Where(recPC => recPC.StockId == rec.Id)
                            .Select(recPC => new StoreRoomEquipmentViewModel
                            {
                                Id = recPC.Id,
                                StockId = recPC.StockId,
                                ComponentId = recPC.ComponentId,
                                ComponentName = source.Components
                                    .FirstOrDefault(recC => recC.Id == recPC.ComponentId)?.ComponentName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public StoreRoomViewModel GetElement(int id)
        {
            StoreRoom element = source.Stocks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StoreRoomViewModel
                {
                    Id = element.Id,
                    StockName = element.StockName,
                    StockComponents = source.StockComponents
                            .Where(recPC => recPC.StockId == element.Id)
                            .Select(recPC => new StoreRoomEquipmentViewModel
                            {
                                Id = recPC.Id,
                                StockId = recPC.StockId,
                                ComponentId = recPC.ComponentId,
                                ComponentName = source.Components
                                    .FirstOrDefault(recC => recC.Id == recPC.ComponentId)?.ComponentName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StoreRoomBindingModel model)
        {
            StoreRoom element = source.Stocks.FirstOrDefault(rec => rec.StockName == model.StockName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Stocks.Count > 0 ? source.Stocks.Max(rec => rec.Id) : 0;
            source.Stocks.Add(new StoreRoom
            {
                Id = maxId + 1,
                StockName = model.StockName
            });
        }

        public void UpdElement(StoreRoomBindingModel model)
        {
            StoreRoom element = source.Stocks.FirstOrDefault(rec =>
                                        rec.StockName == model.StockName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Stocks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StockName = model.StockName;
        }

        public void DelElement(int id)
        {
            StoreRoom element = source.Stocks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // при удалении удаляем все записи о компонентах на удаляемом складе
                source.StockComponents.RemoveAll(rec => rec.StockId == id);
                source.Stocks.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
