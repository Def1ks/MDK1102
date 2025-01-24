namespace БазаДанныхМДК1102.NewFolder1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            ProductMaterial = new HashSet<ProductMaterial>();
        }

        public double ID { get; set; }

        [Required]
        public string Title { get; set; }

        public double? CountInPack { get; set; }

        public string Unit { get; set; }

        public double? CountInStock { get; set; }

        public double? MinCount { get; set; }

        public string Description { get; set; }

        public double? Cost { get; set; }

        public string Image { get; set; }

        public double? MaterialTypeID { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
    }
}
