using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Payment.Persistence.DAO
{
    [ExcludeFromCodeCoverage]
    [Table("Bank")]
    public class Bank
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("IfscCode", TypeName = "varchar(255)")]
        public string IfscCode { get; set; }

        [Column("Name", TypeName = "varchar(255)")]
        public string Name { get; set; }
    }
}