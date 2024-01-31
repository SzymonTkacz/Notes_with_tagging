using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notes_with_tagging.Identity
{
    public class AccessTokenData
    {
        public required string Name { get; set; }

        public required string Surname { get; set; }
    }
}
