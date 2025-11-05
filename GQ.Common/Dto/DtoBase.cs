namespace GQ.Common.Dto;

public interface IDtoBase
{
    long Id { get; set; }
}

public class DtoBase: IDtoBase
{
    public long Id { get; set; }
}