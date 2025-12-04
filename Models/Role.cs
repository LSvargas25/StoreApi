using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace StoreApi.Models
{
    [Table("Role", Schema = "dbo")]  
    public partial class Role
    {
        [Key]  
        public int RoleId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        // Navigation property to UserAccount
        public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
    }
}
