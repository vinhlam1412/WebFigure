namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Histories = new HashSet<History>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int IdOrder { get; set; }

        public int? idPayment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDat { get; set; }

        public int? idDelivery { get; set; }

        public int? idAccount { get; set; }

        public int? IdStatus { get; set; }

        public virtual Account Account { get; set; }

        public virtual Delivery Delivery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> Histories { get; set; }

        public virtual Status Status { get; set; }

        public virtual Payment Payment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
