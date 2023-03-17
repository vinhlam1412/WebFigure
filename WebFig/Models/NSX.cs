namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NSX")]
    public partial class NSX
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NSX()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int idNSX { get; set; }

        [StringLength(50)]
        public string tenNSX { get; set; }

        [StringLength(100)]
        public string hinhNSX { get; set; }

        public string motaNSX { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
