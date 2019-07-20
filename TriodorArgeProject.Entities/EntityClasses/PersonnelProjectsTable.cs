namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PersonnelProjectsTable")]
    public partial class PersonnelProjectsTable
    {
        public int ID { get; set; }

        public int? ProjectID { get; set; }

        public int? PersonnelID { get; set; }

        [StringLength(4)]
        public string WorkRate { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        [StringLength(10)]
        public string Month { get; set; }

        public virtual Personnels Personnels { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
