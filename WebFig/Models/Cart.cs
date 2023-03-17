namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        public int idCart { get; set; }

        public int? idProduct { get; set; }

        public int? idAccount { get; set; }

        public int? soluong { get; set; }

        public decimal? gia { get; set; }

        public virtual Account Account { get; set; }

        public virtual Product Product { get; set; }
    }
}
