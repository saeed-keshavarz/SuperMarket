namespace SuperMarket.Services.Stuffs.Contracts
{
    public class AddStuffDto
    {
        public string Title { get; set; }
        public int Inventory { get; set; }
        public string Unit { get; set; }
        public int MinimumInventory { get; set; }
        public int MaximumInventory { get; set; }
        public int CategoryId { get; set; }
    }
}
