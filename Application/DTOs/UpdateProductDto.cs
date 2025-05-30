namespace Application.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; }
        public List<UpdateItemDto>? Items { get; set; }
    }    
}
