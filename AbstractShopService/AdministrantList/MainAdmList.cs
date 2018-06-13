using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<SafetySystemViewModel> result = new List<SafetySystemViewModel>();
            for (int i = 0; i < source.SafetySystem.Count; ++i)
            {
                string clientFIO = string.Empty;
                for (int j = 0; j < source.Zakazchik.Count; ++j)
                {
                    if(source.Zakazchik[j].Id == source.SafetySystem[i].ClientId)
                    {
                        clientFIO = source.Zakazchik[j].ClientFIO;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.BasicSecurityEquipment.Count; ++j)
                {
                    if (source.BasicSecurityEquipment[j].Id == source.SafetySystem[i].ProductId)
                    {
                        productName = source.BasicSecurityEquipment[j].ProductName;
                        break;
                    }
                }
                string implementerFIO = string.Empty;
                if(source.SafetySystem[i].ImplementerId.HasValue)
                {
                    for (int j = 0; j < source.Administrant.Count; ++j)
                    {
                        if (source.Administrant[j].Id == source.SafetySystem[i].ImplementerId.Value)
                        {
                            implementerFIO = source.Administrant[j].AdministrantFIO;
                            break;
                        }
                    }
                }
                result.Add(new SafetySystemViewModel
                {
                    Id = source.SafetySystem[i].Id,
                    ClientId = source.SafetySystem[i].ClientId,
                    ClientFIO = clientFIO,
                    ProductId = source.SafetySystem[i].ProductId,
                    ProductName = productName,
                    ImplementerId = source.SafetySystem[i].ImplementerId,
                    ImplementerName = implementerFIO,
                    Count = source.SafetySystem[i].Count,
                    Sum = source.SafetySystem[i].Sum,
                    DateCreate = source.SafetySystem[i].DateCreate.ToLongDateString(),
                    DateImplement = source.SafetySystem[i].DateImplement?.ToLongDateString(),
                    Status = source.SafetySystem[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateOrder(SafetySystemBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.SafetySystem.Count; ++i)
            {
                if (source.SafetySystem[i].Id > maxId)
                {
                    maxId = source.Zakazchik[i].Id;
                }
            }
            source.SafetySystem.Add(new SafetySystem
            {
                Id = maxId + 1,
                ClientId = model.ClientId,
                ProductId = model.ProductId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = SafetySystemStatus.Принят
            });
        }

        public void TakeOrderInWork(SafetySystemBM model)
        {
            int index = -1;
            for (int i = 0; i < source.SafetySystem.Count; ++i)
            {
                if (source.SafetySystem[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for(int i = 0; i < source.Equipment_BSEquipment.Count; ++i)
            {
                if(source.Equipment_BSEquipment[i].ProductId == source.SafetySystem[index].ProductId)
                {
                    int countOnStocks = 0;
                    for(int j = 0; j < source.StoreRoomEquipments.Count; ++j)
                    {
                        if(source.StoreRoomEquipments[j].ComponentId == source.Equipment_BSEquipment[i].ComponentId)
                        {
                            countOnStocks += source.StoreRoomEquipments[j].Count;
                        }
                    }
                    if(countOnStocks < source.Equipment_BSEquipment[i].Count * source.SafetySystem[index].Count)
                    {
                        for (int j = 0; j < source.Equipment.Count; ++j)
                        {
                            if (source.Equipment[j].Id == source.Equipment_BSEquipment[i].ComponentId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Equipment[j].ComponentName + 
                                    " требуется " + source.Equipment_BSEquipment[i].Count + ", в наличии " + countOnStocks);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.Equipment_BSEquipment.Count; ++i)
            {
                if (source.Equipment_BSEquipment[i].ProductId == source.SafetySystem[index].ProductId)
                {
                    int countOnStocks = source.Equipment_BSEquipment[i].Count * source.SafetySystem[index].Count;
                    for (int j = 0; j < source.StoreRoomEquipments.Count; ++j)
                    {
                        if (source.StoreRoomEquipments[j].ComponentId == source.Equipment_BSEquipment[i].ComponentId)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.StoreRoomEquipments[j].Count >= countOnStocks)
                            {
                                source.StoreRoomEquipments[j].Count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.StoreRoomEquipments[j].Count;
                                source.StoreRoomEquipments[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.SafetySystem[index].ImplementerId = model.ImplementerId;
            source.SafetySystem[index].DateImplement = DateTime.Now;
            source.SafetySystem[index].Status = SafetySystemStatus.Выполняется;
        }

        public void FinishOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.SafetySystem.Count; ++i)
            {
                if (source.Zakazchik[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.SafetySystem[index].Status = SafetySystemStatus.Готов;
        }

        public void PayOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.SafetySystem.Count; ++i)
            {
                if (source.Zakazchik[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.SafetySystem[index].Status = SafetySystemStatus.Оплачен;
        }

        public void PutComponentOnStock(StoreRoomEquipmentsBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.StoreRoomEquipments.Count; ++i)
            {
                if(source.StoreRoomEquipments[i].StockId == model.StockId && 
                    source.StoreRoomEquipments[i].ComponentId == model.ComponentId)
                {
                    source.StoreRoomEquipments[i].Count += model.Count;
                    return;
                }
                if (source.StoreRoomEquipments[i].Id > maxId)
                {
                    maxId = source.StoreRoomEquipments[i].Id;
                }
            }
            source.StoreRoomEquipments.Add(new StoreRoomEquipments
            {
                Id = ++maxId,
                StockId = model.StockId,
                ComponentId = model.ComponentId,
                Count = model.Count
            });
        }
    }
}
