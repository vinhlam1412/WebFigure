namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("History")]
    public partial class History
    {
        [Key]
        public int idHistory { get; set; }

        public int? idOrder { get; set; }

        public virtual Order Order { get; set; }
    }
}
