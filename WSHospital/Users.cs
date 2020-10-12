namespace WSHospital
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Rendering = new HashSet<Rendering>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Login { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        public double? IP { get; set; }

        public DateTime? Lastenter { get; set; }

        public double? Services { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public int? RoleID { get; set; }

        [StringLength(255)]
        public string Photo { get; set; }

        [StringLength(255)]
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rendering> Rendering { get; set; }

        public virtual Rolee Rolee { get; set; }
    }
}
