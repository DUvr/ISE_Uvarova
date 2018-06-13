using AbstractShopService.ImplementationsList;
using AbstractShopService.Interfaces;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IZakazchikService, ClientServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IEquipmentService, ComponentServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IAdministrantService, ImplementerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBasicSecurityEquipmentService, ProductServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStoreRoomService, StockServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new HierarchicalLifetimeManager());
            
            return currentContainer;
        }
    }
}
