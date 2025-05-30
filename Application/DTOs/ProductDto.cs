namespace Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public List<ItemDto> Items { get; set; } = [];
    }
}