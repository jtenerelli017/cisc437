using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("PHONE_TYPE")]
    public partial class PhoneType
    {
        public PhoneType()
        {
            StudentPhones = new HashSet<StudentPhone>();
        }

        [Key]
        [Column("PHONE_TYPE_ID", TypeName = "NUMBER")]
        public decimal PhoneTypeId { get; set; }
        [Required]
        [Column("PHONE_TYPE")]
        [StringLength(20)]
        public string PhoneType1 { get; set; }

        [InverseProperty(nameof(StudentPhone.PhoneType))]
        public virtual ICollection<StudentPhone> StudentPhones { get; set; }
    }
}
