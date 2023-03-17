namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Coupoun")]
    public partial class Coupoun
    {
        [Key]
        [Column(Order = 0)]
        public int idcode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string codename { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string quantity { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal price { get; set; }
    }
}
