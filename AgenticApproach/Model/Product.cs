namespace AgenticApproach.Model
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int TotalSales { get; set; }
    }
}
