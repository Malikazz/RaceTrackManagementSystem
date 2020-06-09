namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int CarID { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [StringLength(15, ErrorMessage = "Serial number must be less than 16 characters in length.")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Ownership is required")]
        [StringLength(6, ErrorMessage = "Ownership must be less than 7 characters in length.")]
        public string Ownership { get; set; }

        public int CarClassID { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(10, ErrorMessage = "State must be less than 11 characters in length.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Discription must be less than 256 characters in length.")]
        public string Description { get; set; }

        public int? MemberID { get; set; }

        public virtual CarClass CarClass { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
