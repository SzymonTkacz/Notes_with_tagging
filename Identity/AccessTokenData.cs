using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Notes_with_tagging.Identity
{
    public class AccessTokenData
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
