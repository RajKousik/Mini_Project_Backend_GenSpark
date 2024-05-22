using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementApplicationAPI.Models.Db_Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int? HeadId { get; set; }

        [ForeignKey("HeadId")]
        public Faculty Head { get; set; }

        public IList<Student> Students { get; set; }
        public IList<Faculty> Faculties { get; set; }

    }
}
