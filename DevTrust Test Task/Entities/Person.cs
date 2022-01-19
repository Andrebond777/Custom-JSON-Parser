using DevTrust_Test_Task.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevTrust_Test_Task.Entities
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
    }
}
