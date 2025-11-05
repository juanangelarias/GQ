namespace GQ.Common.Dto;

public class ProductDto: DtoBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long ProductTypeId { get; set; }
    
    //

    public ProductTypeDto ProductType { get; set; } = null!;
    public List<ProductPriceDto> Prices { get; set; } = [];
}