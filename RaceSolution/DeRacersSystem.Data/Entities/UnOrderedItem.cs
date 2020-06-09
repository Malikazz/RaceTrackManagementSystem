namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UnOrderedItem
    {
        [Key]
        public int ItemID { get; set; }

        public int OrderID { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(50, ErrorMessage = "Item name must be less than 51 characters in length.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Vendor product Id is required")]
        [StringLength(25, ErrorMessage = "Vendor product Id must be less than 26 characters in length.")]
        public string VendorProductID { get; set; }

        public int Quantity { get; set; }
    }
}
