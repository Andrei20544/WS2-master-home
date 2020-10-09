namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumberAnalyze")]
    public partial class NumberAnalyze
    {
        public int ID { get; set; }

        public int? IDPatient { get; set; }

        public int? IDService { get; set; }

        public virtual LabServices LabServices { get; set; }

        public virtual Patients Patients { get; set; }
    }
}
