namespace Application.DTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public List<CreateItemDto> Items { get; set; } = new();
    }
}