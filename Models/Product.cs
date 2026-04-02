namespace SHOPVN.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public int Stock { get; set; }
        public string ImageEmoji { get; set; } = "📦";
        public string Badge { get; set; } = ""; // new | hot | sale
        public bool IsActive { get; set; } = true;
        public bool IsFlashSale { get; set; } = false;
        public int FlashSaleSold { get; set; } = 0;
        public int FlashSaleTotal { get; set; } = 100;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
