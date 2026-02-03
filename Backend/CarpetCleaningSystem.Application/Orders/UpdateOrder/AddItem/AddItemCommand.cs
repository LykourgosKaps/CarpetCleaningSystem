namespace CarpetCleaningSystem.Application.Orders.UpdateOrder.AddItem
{
    public class AddItemCommand
    {
        public int OrderId { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public int Material { get; set; }
        public int CleaningType { get; set; }
    }
}
