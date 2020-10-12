namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Orderr")]
    public partial class Orderr
    {
        public int ID { get; set; }

        public int? IDPatient { get; set; }

        public int? IDService { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

        public virtual LabServices LabServices { get; set; }

        public virtual Patients Patients { get; set; }
    }
}
