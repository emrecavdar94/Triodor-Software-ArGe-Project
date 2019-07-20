namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SuppProjectCosts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SuppProjectCosts()
        {
            ArgeCost = new HashSet<ArgeCost>();
            EquipmentCosts = new HashSet<EquipmentCosts>();
            MaterialCosts = new HashSet<MaterialCosts>();
            PersonnelCosts = new HashSet<PersonnelCosts>();
            ServiceCosts = new HashSet<ServiceCosts>();
            TravelCosts = new HashSet<TravelCosts>();
        }

        public int ID { get; set; }

        public int? ProjectID { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        [StringLength(1)]
        public string Period { get; set; }

        public decimal? Planned { get; set; }

        public decimal? Actual { get; set; }

        public decimal? Accepted { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArgeCost> ArgeCost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EquipmentCosts> EquipmentCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialCosts> MaterialCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonnelCosts> PersonnelCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceCosts> ServiceCosts { get; set; }

        public virtual SuppProjects SuppProjects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelCosts> TravelCosts { get; set; }
    }
}
