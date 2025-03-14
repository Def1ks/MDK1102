namespace БазаДанныхМДК1102.NewFolder1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductMaterial")]
    public partial class ProductMaterial
    {
        [Key]
        [Column(Order = 0)]
        public double ProductID { get; set; }

        [Key]
        [Column(Order = 1)]
        public double MaterialID { get; set; }

        public double? Count { get; set; }

        public virtual Material Material { get; set; }

        public virtual Product Product { get; set; }
    }
}
