namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReturnOrderItem
    {
        public int ReturnOrderItemID { get; set; }

        public int ReceiveOrderID { get; set; }

        public int? OrderDetailID { get; set; }

        [StringLength(50, ErrorMessage = "Unordered item must be less than 51 characters in length.")]
        public string UnOrderedItem { get; set; }

        public int ItemQuantity { get; set; }

        [StringLength(100, ErrorMessage = "Character must be less than 101 characters in length.")]
        public string Comment { get; set; }

        [StringLength(25, ErrorMessage = "Vendor product Id must be less than 26 characters in length.")]
        public string VendorProductID { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }

        public virtual ReceiveOrder ReceiveOrder { get; set; }
    }
}
