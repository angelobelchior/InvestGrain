namespace InvestGrain.Contracts.Models;

[GenerateSerializer, Alias("InvestGrain.Contracts.Models.Stock")]
public class Stock
{
    [Id(0)] public string Name { get; set; } = string.Empty;
    [Id(1)] public decimal Value { get; set; }
    [Id(2)] public DateTime DateTime { get; set; }

    public static string[] GetStockNameList() =>
    [
        "PETR3",
        "PETR4",
        "VALE3",
        "ITUB3",
        "ITUB4",
        "BBDC3",
        "BBDC4",
        "ABEV3",
        "BBAS3",
        "WEGE3",
        "MGLU3",
        "B3SA3",
        "ELET3",
        "ELET6",
    ];
}