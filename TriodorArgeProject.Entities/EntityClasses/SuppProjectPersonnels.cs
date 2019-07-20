namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SuppProjectPersonnels
    {
        public int ID { get; set; }

        public int PersonnelID { get; set; }

        public int SuppProjectD { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public virtual Personnels Personnels { get; set; }

        public virtual SuppProjects SuppProjects { get; set; }
    }
}
