namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LabServices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LabServices()
        {
            NumberAnalyze = new HashSet<NumberAnalyze>();
            Orderr = new HashSet<Orderr>();
            Rendering = new HashSet<Rendering>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public double? Cost { get; set; }

        public DateTime? Period { get; set; }

        [StringLength(255)]
        public string MeanDeviation { get; set; }

        public double? ServiceCode { get; set; }

        public int? IDSetService { get; set; }

        public virtual SetServicee SetServicee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumberAnalyze> NumberAnalyze { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orderr> Orderr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rendering> Rendering { get; set; }
    }
}
