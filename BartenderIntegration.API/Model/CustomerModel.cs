namespace BartenderIntegration.API.Model
{
    public class CustomerModel
    {
        public string LicenseNo { get; set; } = string.Empty;
        public string ProductionOrderNo { get; set; } = string.Empty;
        public string ItemNo { get; set; } = string.Empty;
        public string BatchNo { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string ItemDesc { get; set; } = string.Empty;
        public string SerialNo { get; set; } = string.Empty;
        public string ConfigurationId { get; set; } = string.Empty;
    }
}
