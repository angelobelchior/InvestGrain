namespace InvestGrain.Contracts.Models;

[GenerateSerializer, Alias("InvestGrain.Contracts.Models.Stock")]
public class Stock
{
    [Id(0)] public string Name { get; set; } = string.Empty;
    [Id(1)] public string Symbol { get; set; } = string.Empty;
    [Id(2)] public string Icon { get; set; } = string.Empty;
    [Id(3)] public decimal Value { get; set; }
    [Id(4)] public decimal Change { get; set; }
    [Id(5)] public DateTime DateTime { get; set; }

    public static (string Symbol, string Name)[] ListAll() =>
    [
        ("PETR3", "Petróleo Brasileiro S.A"),
        ("PETR4", "Petróleo Brasileiro S.A"),
        ("VALE3", "Vale"),
        ("ITUB3", "Itaú Unibanco"),
        ("ITUB4", "Itaú Unibanco"),
        ("BBDC3", "Banco Bradesco"),
        ("BBDC4", "Banco Bradesco"),
        ("ABEV3", "Ambev"),
        ("BBAS3", "Banco do Brasil"),
        ("WEGE3", "WEG"),
        ("MGLU3", "Magazine Luiza"),
        ("B3SA3", "B3"),
        ("ELET3", "Eletrobras"),
        ("ELET6", "Eletrobras"),
    ];
}