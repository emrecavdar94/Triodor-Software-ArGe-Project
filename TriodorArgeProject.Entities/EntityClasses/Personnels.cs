namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Personnels
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personnels()
        {
            PersonnelAbility = new HashSet<PersonnelAbility>();
            PersonnelCertificates = new HashSet<PersonnelCertificates>();
            PersonnelCosts = new HashSet<PersonnelCosts>();
            PersonnelEducations = new HashSet<PersonnelEducations>();
            PersonnelProjectsTable = new HashSet<PersonnelProjectsTable>();
            SuppProjectPersonnels = new HashSet<SuppProjectPersonnels>();
            TravelCosts = new HashSet<TravelCosts>();
            WorkExperiences = new HashSet<WorkExperiences>();
        }

        public int Id { get; set; }

        public int PositionID { get; set; }

        public int? UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string RegisterNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeaveDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelAbility> PersonnelAbility { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelCertificates> PersonnelCertificates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelCosts> PersonnelCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelEducations> PersonnelEducations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelProjectsTable> PersonnelProjectsTable { get; set; }

        public virtual Positions Positions { get; set; }

        public virtual Users Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuppProjectPersonnels> SuppProjectPersonnels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelCosts> TravelCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkExperiences> WorkExperiences { get; set; }
    }
}
