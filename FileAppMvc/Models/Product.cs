namespace FileAppMvc.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }    = "test";
        public int Amount { get; set; }     = 10;
        public double Price { get; set; }   = 100;
        public string? Image { get; set; }   = string.Empty;
        public string? ImageBase64 { get; set; } = string.Empty;
    }
}
