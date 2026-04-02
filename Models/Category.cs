namespace SHOPVN.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = " ";
        public string Icon { get; set; } = " ";
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
