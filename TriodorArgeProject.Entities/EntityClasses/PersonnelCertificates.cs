namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PersonnelCertificates
    {
        public int ID { get; set; }

        public int PersonnelID { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public virtual Personnels Personnels { get; set; }
    }
}
