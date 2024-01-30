using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellIntegro.Models
{
    [Table("Inscriptions")]
    public class Inscription
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Text")]
        public required string Text { get; set; }

        [Column("Tags")]
        public string[]? Tags { get; set; }
    }
}
