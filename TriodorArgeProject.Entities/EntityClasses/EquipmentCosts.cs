namespace TriodorArgeProject.Entities.EntityClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EquipmentCosts
    {
        public int ID { get; set; }

        public int? CostID { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public int? Count { get; set; }

        public decimal? Actual { get; set; }

        public decimal? Planned { get; set; }

        public decimal? Accepted { get; set; }

        public string Note { get; set; }

        public virtual SuppProjectCosts SuppProjectCosts { get; set; }
    }
}
