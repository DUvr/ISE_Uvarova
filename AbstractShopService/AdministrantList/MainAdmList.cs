using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<SafetySystemViewModel> GetList()
        {
            List<SafetySystemViewModel> result = source.Orders
                .Select(rec => new SafetySystemViewModel
                {
                    Id = rec.Id,
                    ClientId = rec.ClientId,
                    ProductId = rec.ProductId,
                    ImplementerId = rec.ImplementerId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    ClientFIO = source.Clients
                                    .FirstOrDefault(recC => recC.Id == rec.ClientId)?.ZakazchikFIO,
                    ProductName = source.Products
                                    .FirstOrDefault(recP => recP.Id == rec.ProductId)?.ProductName,
                    ImplementerName = source.Administrant
                                    .FirstOrDefault(recI => recI.Id == rec.ImplementerId)?.AdministrantFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrder(SafetySystemBindingModel model)
        {
            int maxId = source.Orders.Count > 0 ? source.Orders.Max(rec => rec.Id) : 0;
            source.Orders.Add(new SafetySystem
            {
                Id = maxId + 1,
                ClientId = model.ClientId,
                ProductId = model.ProductId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = SafetySystemStatusStatus.Принят
            });
        }

        public void TakeOrderInWork(SafetySystemBindingModel model)
        {
            SafetySystem element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var productComponents = source.ProductComponents.Where(rec => rec.ProductId == element.ProductId);
            foreach(var productComponent in productComponents)
            {
                int countOnStocks = source.StockComponents
                                            .Where(rec => rec.ComponentId == productComponent.ComponentId)
                                            .Sum(rec => rec.Count);
                if (countOnStocks < productComponent.Count * element.Count)
                {
                    var componentName = source.Components
                                    .FirstOrDefault(rec => rec.Id == productComponent.ComponentId);
                    throw new Exception("Не достаточно компонента " + componentName?.ComponentName +
                        " требуется " + productComponent.Count + ", в наличии " + countOnStocks);
                }
            }
            // списываем
            foreach (var productComponent in productComponents)
            {
                int countOnStocks = productComponent.Count * element.Count;
                var stockComponents = source.StockComponents
                                            .Where(rec => rec.ComponentId == productComponent.ComponentId);
                foreach (var stockComponent in stockComponents)
                {
                    // компонентов на одном слкаде может не хватать
                    if (stockComponent.Count >= countOnStocks)
                    {
                        stockComponent.Count -= countOnStocks;
                        break;
                    }
                    else
                    {
                        countOnStocks -= stockComponent.Count;
                        stockComponent.Count = 0;
                    }
                }
            }
            element.ImplementerId = model.ImplementerId;
            element.DateImplement = DateTime.Now;
            element.Status = SafetySystemStatusStatus.Выполняется;
        }

        public void FinishOrder(int id)
        {
            SafetySystem element = source.Orders.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = SafetySystemStatusStatus.Готов;
        }

        public void PayOrder(int id)
        {
            SafetySystem element = source.Orders.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = SafetySystemStatusStatus.Оплачен;
        }

        public void PutComponentOnStock(StoreRoomEquipmentBindingModel model)
        {
            StoreRoomEquipment element = source.StockComponents
                                                .FirstOrDefault(rec => rec.StockId == model.StockId && 
                                                                    rec.ComponentId == model.ComponentId);
            if(element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StockComponents.Count > 0 ? source.StockComponents.Max(rec => rec.Id) : 0;
                source.StockComponents.Add(new StoreRoomEquipment
                {
                    Id = ++maxId,
                    StockId = model.StockId,
                    ComponentId = model.ComponentId,
                    Count = model.Count
                });
            }
        }
    }
}
