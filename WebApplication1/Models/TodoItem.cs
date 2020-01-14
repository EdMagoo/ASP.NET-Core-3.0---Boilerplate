using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL;

namespace WebApplication1.Models
{
    [Table("todo")]
    public class TodoItem: IEntity
    {
        [Column(name: "id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(name: "title")]
        [StringLength(255)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Column(name: "description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Column(name: "start_date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; } 
        // remember that in mysql, if we want to store the fractional part we must declare DATETIME(6)
        // AVOID TO USE TIMESTAMP

        [Column(name: "end_date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Column(name: "is_completed")]
        public bool IsCompleted { get; set; } = false;
    }
}
