namespace WorkerService.Application.Dtos;

public class AssetDto
{
    public decimal? Price { get; set; }
    public decimal? PriceOpen { get; set; }
    public decimal? High { get; set; }
    public decimal? Low { get; set; }
    public int? Volume { get; set; }
    public long? MarketCap { get; set; }
    public DateTime? TradeTime { get; set; }
    public int? VolumeAvg { get; set; }
    public decimal? Pe { get; set; }
    public decimal? Eps { get; set; }
    public decimal? High52 { get; set; }
    public decimal? Low52 { get; set; }
    public decimal? Change { get; set; }
    public decimal? ChangePct { get; set; }
    public decimal? CloseYest { get; set; }
    public long? Shares { get; set; }
    public string? Ticker { get; set; }
}