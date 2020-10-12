namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rendering")]
    public partial class Rendering
    {
        public int ID { get; set; }

        public int? IDService { get; set; }

        public DateTime? Period { get; set; }

        public int? UserID { get; set; }

        [StringLength(255)]
        public string Analyzer { get; set; }

        public virtual LabServices LabServices { get; set; }

        public virtual Users Users { get; set; }
    }
}
