namespace BackEnd.Data
{
    public class InvoiceData {
        public required string Title { get; set; }
        public required string Address { get; set; }
        public required string ZipCode { get; set; }
        public required string Country { get; set; }
        public required int InvoiceNumber { get; set; }
        public required DateTime DateCreated { get; set; }
        public required DateTime DateDue { get; set; }
        public string? Condition { get; set; }
        public string? DelayFine { get; set; }
        public string Font { get; set; }
    }
}