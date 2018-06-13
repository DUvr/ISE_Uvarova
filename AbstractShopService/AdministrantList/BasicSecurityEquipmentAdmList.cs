using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class ProductServiceList : IBasicSecurityEquipmentService
    {
        private DataListSingleton source;

        public ProductServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<BasicSecurityEquipmentViewModel> GetList()
        {
            List<BasicSecurityEquipmentViewModel> result = new List<BasicSecurityEquipmentViewModel>();
            for (int i = 0; i < source.BasicSecurityEquipment.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<Equipment_BSEquipmentViewModel> productComponents = new List<Equipment_BSEquipmentViewModel>();
                for (int j = 0; j < source.Equipment_BSEquipment.Count; ++j)
                {
                    if (source.Equipment_BSEquipment[j].ProductId == source.BasicSecurityEquipment[i].Id)
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
                        productComponents.Add(new Equipment_BSEquipmentViewModel
                        {
                            Id = source.Equipment_BSEquipment[j].Id,
                            ProductId = source.Equipment_BSEquipment[j].ProductId,
                            ComponentId = source.Equipment_BSEquipment[j].ComponentId,
                            ComponentName = componentName,
                            Count = source.Equipment_BSEquipment[j].Count
                        });
                    }
                }
                result.Add(new BasicSecurityEquipmentViewModel
                {
                    Id = source.BasicSecurityEquipment[i].Id,
                    ProductName = source.BasicSecurityEquipment[i].ProductName,
                    Price = source.BasicSecurityEquipment[i].Price,
                    ProductComponents = productComponents
                });
            }
            return result;
        }

        public BasicSecurityEquipmentViewModel GetElement(int id)
        {
            for (int i = 0; i < source.BasicSecurityEquipment.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<Equipment_BSEquipmentViewModel> productComponents = new List<Equipment_BSEquipmentViewModel>();
                for (int j = 0; j < source.Equipment_BSEquipment.Count; ++j)
                {
                    if (source.Equipment_BSEquipment[j].ProductId == source.BasicSecurityEquipment[i].Id)
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
                        productComponents.Add(new Equipment_BSEquipmentViewModel
                        {
                            Id = source.Equipment_BSEquipment[j].Id,
                            ProductId = source.Equipment_BSEquipment[j].ProductId,
                            ComponentId = source.Equipment_BSEquipment[j].ComponentId,
                            ComponentName = componentName,
                            Count = source.Equipment_BSEquipment[j].Count
                        });
                    }
                }
                if (source.BasicSecurityEquipment[i].Id == id)
                {
                    return new BasicSecurityEquipmentViewModel
                    {
                        Id = source.BasicSecurityEquipment[i].Id,
                        ProductName = source.BasicSecurityEquipment[i].ProductName,
                        Price = source.BasicSecurityEquipment[i].Price,
                        ProductComponents = productComponents
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(BasicSecurityEquipmentBM model)
        {
            int maxId = 0;
            for (int i = 0; i < source.BasicSecurityEquipment.Count; ++i)
            {
                if (source.BasicSecurityEquipment[i].Id > maxId)
                {
                    maxId = source.BasicSecurityEquipment[i].Id;
                }
                if (source.BasicSecurityEquipment[i].ProductName == model.ProductName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.BasicSecurityEquipment.Add(new BasicSecurityEquipment
            {
                Id = maxId + 1,
                ProductName = model.ProductName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.Equipment_BSEquipment.Count; ++i)
            {
                if (source.Equipment_BSEquipment[i].Id > maxPCId)
                {
                    maxPCId = source.Equipment_BSEquipment[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.ProductComponents.Count; ++i)
            {
                for (int j = 1; j < model.ProductComponents.Count; ++j)
                {
                    if(model.ProductComponents[i].ComponentId ==
                        model.ProductComponents[j].ComponentId)
                    {
                        model.ProductComponents[i].Count +=
                            model.ProductComponents[j].Count;
                        model.ProductComponents.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.ProductComponents.Count; ++i)
            {
                source.Equipment_BSEquipment.Add(new Equipment_BSEquipment
                {
                    Id = ++maxPCId,
                    ProductId = maxId + 1,
                    ComponentId = model.ProductComponents[i].ComponentId,
                    Count = model.ProductComponents[i].Count
                });
            }
        }

        public void UpdElement(BasicSecurityEquipmentBM model)
        {
            int index = -1;
            for (int i = 0; i < source.BasicSecurityEquipment.Count; ++i)
            {
                if (source.BasicSecurityEquipment[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.BasicSecurityEquipment[i].ProductName == model.ProductName && 
                    source.BasicSecurityEquipment[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.BasicSecurityEquipment[index].ProductName = model.ProductName;
            source.BasicSecurityEquipment[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.Equipment_BSEquipment.Count; ++i)
            {
                if (source.Equipment_BSEquipment[i].Id > maxPCId)
                {
                    maxPCId = source.Equipment_BSEquipment[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.Equipment_BSEquipment.Count; ++i)
            {
                if (source.Equipment_BSEquipment[i].ProductId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.ProductComponents.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.Equipment_BSEquipment[i].Id == model.ProductComponents[j].Id)
                        {
                            source.Equipment_BSEquipment[i].Count = model.ProductComponents[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if(flag)
                    {
                        source.Equipment_BSEquipment.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for(int i = 0; i < model.ProductComponents.Count; ++i)
            {
                if(model.ProductComponents[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.Equipment_BSEquipment.Count; ++j)
                    {
                        if (source.Equipment_BSEquipment[j].ProductId == model.Id &&
                            source.Equipment_BSEquipment[j].ComponentId == model.ProductComponents[i].ComponentId)
                        {
                            source.Equipment_BSEquipment[j].Count += model.ProductComponents[i].Count;
                            model.ProductComponents[i].Id = source.Equipment_BSEquipment[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.ProductComponents[i].Id == 0)
                    {
                        source.Equipment_BSEquipment.Add(new Equipment_BSEquipment
                        {
                            Id = ++maxPCId,
                            ProductId = model.Id,
                            ComponentId = model.ProductComponents[i].ComponentId,
                            Count = model.ProductComponents[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.Equipment_BSEquipment.Count; ++i)
            {
                if (source.Equipment_BSEquipment[i].ProductId == id)
                {
                    source.Equipment_BSEquipment.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.BasicSecurityEquipment.Count; ++i)
            {
                if (source.BasicSecurityEquipment[i].Id == id)
                {
                    source.BasicSecurityEquipment.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
