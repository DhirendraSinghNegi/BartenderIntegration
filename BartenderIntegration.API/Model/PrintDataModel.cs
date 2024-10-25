namespace BartenderIntegration.API.Model
{
    public class PrintDataModel
    {
        public string Colour { get; set; } = string.Empty;
        public string InCtnBarcode { get; set; } = string.Empty;
        public string InventLocationId { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string LabelType { get; set; } = string.Empty;
        public string LineDrawing { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string OutCtnBarcode { get; set; } = string.Empty;
        public string OuterQuantity { get; set; } = string.Empty;
        public string PrinterName { get; set; } = string.Empty;
        public string QuantityLabel { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public string WelsLabel { get; set; } = string.Empty;
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }

    }
}
