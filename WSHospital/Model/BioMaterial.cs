namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BioMaterial")]
    public partial class BioMaterial
    {
        [Key]
        public int IDBio { get; set; }

        public int? BioCode { get; set; }

        [StringLength(255)]
        public string BioName { get; set; }

        [Column("BioMaterial")]
        [StringLength(255)]
        public string BioMaterial1 { get; set; }

        [StringLength(255)]
        public string Descript { get; set; }

        public int? IDSetService { get; set; }

        public virtual SetServicee SetServicee { get; set; }
    }
}
