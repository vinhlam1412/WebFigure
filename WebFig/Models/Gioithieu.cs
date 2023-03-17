namespace WebFig.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Gioithieu")]
    public partial class Gioithieu
    {
        [Key]
        public int idBaiviet { get; set; }

        [StringLength(50)]
        public string Topic1 { get; set; }

        [StringLength(50)]
        public string Topic2 { get; set; }

        [StringLength(50)]
        public string Topic3 { get; set; }

        [StringLength(50)]
        public string Topic4 { get; set; }

        [StringLength(50)]
        public string Topic5 { get; set; }

        [StringLength(50)]
        public string Topic6 { get; set; }

        [StringLength(50)]
        public string Topic7 { get; set; }

        [StringLength(50)]
        public string Topic8 { get; set; }

        [StringLength(50)]
        public string Topic9 { get; set; }

        [StringLength(50)]
        public string Topic10 { get; set; }

        public string Para { get; set; }

        public string Para2 { get; set; }

        public string Para3 { get; set; }

        public string Para4 { get; set; }

        public string Para5 { get; set; }

        public string Para6 { get; set; }

        public string Para7 { get; set; }

        public string Para8 { get; set; }
    }
}
