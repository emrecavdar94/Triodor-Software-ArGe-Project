namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PersonnelCosts
    {
        public int ID { get; set; }

        public int? CostID { get; set; }

        public int? PersonnelID { get; set; }

        public decimal? ActualManMonth { get; set; }

        public decimal? PlannedManMonth { get; set; }

        public decimal? AcceptedManMonth { get; set; }

        public string Notes { get; set; }

        public virtual Personnels Personnels { get; set; }

        public virtual SuppProjectCosts SuppProjectCosts { get; set; }
    }
}
