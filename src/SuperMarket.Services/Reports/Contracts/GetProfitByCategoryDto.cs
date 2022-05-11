namespace SuperMarket.Services.Reports.Contracts
{
    public class GetProfitByCategoryDto
    {
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public decimal Income { get; set; }
        public decimal Profit { get; set; }
    }
}
