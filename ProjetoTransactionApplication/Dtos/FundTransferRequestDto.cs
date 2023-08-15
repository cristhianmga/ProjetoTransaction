

namespace ProjetoTransactionApplication.Dtos
{
    public class FundTransferRequestDto
    {
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }
    }
}
