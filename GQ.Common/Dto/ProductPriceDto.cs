namespace GQ.Common.Dto;

public class ProductPriceDto: DtoBase
{
    public long ProductId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public decimal Price { get; set; }
    
    //

    public ProductDto Product { get; set; } = null!;
}