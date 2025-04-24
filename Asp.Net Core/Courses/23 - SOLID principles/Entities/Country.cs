using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Domain model for Country
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        // this is required to establish a one-to-many relationship when it's used as a foreign key in the Person class
        public virtual ICollection<Person>? Persons { get; set; }
    }
}
