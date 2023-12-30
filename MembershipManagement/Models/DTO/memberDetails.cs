using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MembershipManagement.Models.DTO
{
    [Table("MemberDetails")]
    public class memberDetails
    {
        [Column(TypeName = "int"),Required,Key]
        public int Id { get; set; }

        [Column(TypeName ="nvarchar(10)"),Required]
        public string MemberId { get; set; }

        [Column(TypeName = "nvarchar(100)"), Required]
        public string FullName { get; set; }

        [Column(TypeName = "datetime"), Required]
        public DateTime DOB { get; set; }

        [Column(TypeName = "varchar(100"), Required]
        public string Email { get; set; }

        [Column(TypeName = "varchar(30)"), Required]
        public string Password { get; set; }

        [Column(TypeName = "varchar(15)"), Required]
        public string MobileNumber { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? OtherNumber { get; set; }

        [Column(TypeName = "varchar(10)"), Required]
        public string Gender { get; set; }

        [Column(TypeName = "varchar(max)"), Required]
        public string Address { get; set; }

        [Column(TypeName = "datetime"), Required]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime"), Required]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "varchar(max)"),Required]
        public string PackageDetails { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? PurchaseDate { get; set; }

        [Column(TypeName = "varchar(100)"), Required]
        public string PurchaseBy { get; set; }

        [Column(TypeName = "datetime"), Required]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "varchar(20)"), Required]
        public string Status { get; set; }

        [Column(TypeName = "bit"), Required]
        public bool IsActive { get; set; }
    }
}
