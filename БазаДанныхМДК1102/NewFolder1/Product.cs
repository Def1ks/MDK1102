namespace БазаДанныхМДК1102.NewFolder1
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
            ProductMaterial = new HashSet<ProductMaterial>();
        }

        public double ID { get; set; }

        [Required]
        public string Title { get; set; }

        public double? ProductTypeID { get; set; }

        public double? ArticleNumber { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public double? ProductionPersonCount { get; set; }

        public double? ProductionWorkshopNumber { get; set; }

        public double? MinCostForAgent { get; set; }

        public virtual ProductType ProductType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
    }
}
