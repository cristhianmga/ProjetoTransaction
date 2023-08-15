

using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoTransactionDomain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        public int Status { get; set; }
        public string? Error { get; set; }
    }
}
