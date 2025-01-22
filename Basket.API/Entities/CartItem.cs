namespace Basket.API.Entities
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string No { get; set; }
        public string Name { get; set; }

        public int AvailableStock { get;  set; }

        public void SetAvailableStock(int value) => (AvailableStock) = value;
    }
}
