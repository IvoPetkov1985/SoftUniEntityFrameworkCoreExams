using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required]
        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public virtual Task Task { get; set; } = null!;
    }
}

//•	EmployeeId – integer, Primary Key, foreign key (required)
//•	Employee – Employee
//•	TaskId – integer, Primary Key, foreign key (required)
//•	Task – Task
