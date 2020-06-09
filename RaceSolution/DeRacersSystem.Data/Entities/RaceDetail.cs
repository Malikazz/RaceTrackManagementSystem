namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RaceDetail
    {
        public int RaceDetailID { get; set; }

        public int RaceID { get; set; }

        public int? CarID { get; set; }

        public int MemberID { get; set; }

        public int? Place { get; set; }

        public TimeSpan? RunTime { get; set; }

        public int? PenaltyID { get; set; }

        [StringLength(1048, ErrorMessage = "Comment must be less than 1049 characters in length.")]
        public string Comment { get; set; }

        public int? InvoiceID { get; set; }

        [Column(TypeName = "money")]
        public decimal RaceFee { get; set; }

        [Column(TypeName = "money")]
        public decimal RentalFee { get; set; }

        public bool Refund { get; set; }

        [StringLength(150, ErrorMessage = "Refund reason must be less than 151 characters in length.")]
        public string RefundReason { get; set; }

        public virtual Car Car { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Member Member { get; set; }

        public virtual Race Race { get; set; }

        public virtual RacePenalty RacePenalty { get; set; }
    }
}
