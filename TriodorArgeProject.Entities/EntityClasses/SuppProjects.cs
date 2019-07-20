namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SuppProjects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SuppProjects()
        {
            SuppProjectCosts = new HashSet<SuppProjectCosts>();
            SuppProjectPersonnels = new HashSet<SuppProjectPersonnels>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string ProjectNo { get; set; }

        public int? FundingSchemesID { get; set; }

        public int? StatusID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AppealDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SignedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SuppStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SuppEndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PlannedManMonth { get; set; }

        public decimal Budget { get; set; }

        public int SupportRate { get; set; }

        public virtual FundingSchemes FundingSchemes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuppProjectCosts> SuppProjectCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuppProjectPersonnels> SuppProjectPersonnels { get; set; }

        public virtual SuppProjectsStatus SuppProjectsStatus { get; set; }
    }
}
