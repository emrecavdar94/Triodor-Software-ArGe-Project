namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PersonnelEducations
    {
        public int ID { get; set; }

        public int PersonnelID { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(4)]
        public string StartYear { get; set; }

        [StringLength(4)]
        public string EndYear { get; set; }

        public virtual Personnels Personnels { get; set; }
    }
}
