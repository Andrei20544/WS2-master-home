namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Patients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patients()
        {
            NumberAnalyze = new HashSet<NumberAnalyze>();
            Orderr = new HashSet<Orderr>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string Login { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string FIO { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string PassportData { get; set; }

        public double? Phone { get; set; }

        public double? InsurancePolicy { get; set; }

        [StringLength(255)]
        public string TypeOfPolicy { get; set; }

        public int? IDCompany { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumberAnalyze> NumberAnalyze { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orderr> Orderr { get; set; }
    }
}
