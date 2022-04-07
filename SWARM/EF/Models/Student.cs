using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("STUDENT")]
    public partial class Student
    {
        public Student()
        {
            StudentPhones = new HashSet<StudentPhone>();
        }

        [Key]
        [Column("STUDENT_ID", TypeName = "NUMBER")]
        public decimal StudentId { get; set; }
        [Required]
        [Column("STUDENT_NAME")]
        [StringLength(30)]
        public string StudentName { get; set; }

        [InverseProperty(nameof(StudentPhone.Student))]
        public virtual ICollection<StudentPhone> StudentPhones { get; set; }
    }
}
