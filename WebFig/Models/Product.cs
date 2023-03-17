namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int idProduct { get; set; }

        [Required]
        [StringLength(100)]
        public string ten { get; set; }

        public decimal? gia { get; set; }

        [StringLength(100)]
        public string hinh { get; set; }

        [Required]
        public string mota { get; set; }

        public int? soluongton { get; set; }

        public int? idNSX { get; set; }

        public int? idCategory { get; set; }

        [StringLength(100)]
        public string hinhDetail1 { get; set; }

        [StringLength(100)]
        public string hinhDetail2 { get; set; }

        [StringLength(100)]
        public string hinhDetail3 { get; set; }

        public int? idSize { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual Category Category { get; set; }

        public virtual NSX NSX { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Size Size { get; set; }
    }
}
