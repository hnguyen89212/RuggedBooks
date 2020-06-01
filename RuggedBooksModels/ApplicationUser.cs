using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RuggedBooksModels
{
    // There is a table dbo.AspNetUser out of the box.
    // We would like to extend that table.
    // We need to install package Microsoft.Extensions.Identity.Stores
    // With that, every additional property inside this class will be added
    // to the table as new column.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        // We do not a Role column yet, so make this as not mapped.
        [NotMapped]
        public string Role { get; set; }

        // Some users are Company, others are individuals.
        // Hence, the CompanyId is nullable.
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
}

/**
 * There is an additional column called Discriminator to distinguish between the AspNetUser or custom ApplicationUser.
 */
