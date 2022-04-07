using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("STUDENT_PHONE")]
    public partial class StudentPhone
    {
        [Key]
        [Column("STUDENT_PHONE_ID", TypeName = "NUMBER")]
        public decimal StudentPhoneId { get; set; }
        [Column("STUDENT_ID", TypeName = "NUMBER")]
        public decimal StudentId { get; set; }
        [Column("PHONE_TYPE_ID", TypeName = "NUMBER")]
        public decimal PhoneTypeId { get; set; }
        [Required]
        [Column("PHONE_NUMBER")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Required]
        [Column("STUDENT_PHONE_PRIMARY")]
        [StringLength(1)]
        public string StudentPhonePrimary { get; set; }

        [ForeignKey(nameof(PhoneTypeId))]
        [InverseProperty("StudentPhones")]
        public virtual PhoneType PhoneType { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty("StudentPhones")]
        public virtual Student Student { get; set; }
    }
}
