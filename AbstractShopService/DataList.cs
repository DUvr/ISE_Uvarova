using AbstractShopModel;
using System.Collections.Generic;

namespace AbstractShopService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Zakazchik> Zakazchik { get; set; }

        public List<Equipment> Equipment { get; set; }

        public List<Administrant> Administrant { get; set; }

        public List<SafetySystem> SafetySystem { get; set; }

        public List<BasicSecurityEquipment> BasicSecurityEquipment { get; set; }

        public List<Equipment_BSEquipment> Equipment_BSEquipment { get; set; }

        public List<StoreRoom> StoreRoom { get; set; }

        public List<StoreRoomEquipments> StoreRoomEquipments { get; set; }

        private DataListSingleton()
        {
            Zakazchik = new List<Zakazchik>();
            Equipment = new List<Equipment>();
            Administrant = new List<Administrant>();
            SafetySystem = new List<SafetySystem>();
            BasicSecurityEquipment = new List<BasicSecurityEquipment>();
            Equipment_BSEquipment = new List<Equipment_BSEquipment>();
            StoreRoom = new List<StoreRoom>();
            StoreRoomEquipments = new List<StoreRoomEquipments>();
        }

        public static DataListSingleton GetInstance()
        {
            if(instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
