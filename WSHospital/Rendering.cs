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
        public int Id { get; set; }

        public int IdService { get; set; }

        [StringLength(255)]
        public string Period { get; set; }

        public int UserId { get; set; }

        [StringLength(255)]
        public string Analyzer { get; set; }
    }
}
