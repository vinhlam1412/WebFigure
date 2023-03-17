namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        [Key]
        public int idAccount { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Không đượcc bỏ trống")]
        [Display(Name = "Tên tài khoản")]
        public string username { get; set; }

        [Required(ErrorMessage = "Không đượcc bỏ trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Không đượcc bỏ trống")]

        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]

        [Compare("password", ErrorMessage = "Mật khẩu không trùng khớp")]

        public string password_verify { get; set; }


        [StringLength(100)]
        [Required(ErrorMessage = "Không đượcc bỏ trống")]
        [Display(Name = "Họ tên")]
        public string Hoten { get; set; }


        [Required(ErrorMessage = "Không đượcc bỏ trống")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Vui lòng nhập số")]
        [StringLength(11, ErrorMessage = "Số điện thoại nhập không đủ", MinimumLength = 10)]


        [Display(Name = "Số điện thoại")]
        public string SoDT { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Không được bỏ trống")]
        [Display(Name = "Địa chỉ")]
        public string Diachi { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Bạn chưa nhập email !!!")]
        public string Email { get; set; }

        public bool IsValid1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
