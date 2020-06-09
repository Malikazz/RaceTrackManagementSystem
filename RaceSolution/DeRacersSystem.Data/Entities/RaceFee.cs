namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RaceFee
    {
        public int RaceFeeID { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(50, ErrorMessage = "Description must be less than 51 characters in length.")]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Fee { get; set; }
    }
}
