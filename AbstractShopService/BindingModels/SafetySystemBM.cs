namespace AbstractShopService.BindingModels
{
    public class SafetySystemBM
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ProductId { get; set; }

        public int? ImplementerId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
