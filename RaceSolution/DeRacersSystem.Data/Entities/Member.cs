namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Cars = new HashSet<Car>();
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int MemberID { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(30, ErrorMessage = "Last name must be less than 31 characters in length.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, ErrorMessage = "First name must be less than 31 characters in length.")]
        public string FirstName { get; set; }

        [StringLength(10, ErrorMessage = "Phone must be less than 11 characters in length.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(30, ErrorMessage = "Address must be less than 31 characters in length.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, ErrorMessage = "City must be less than 31 characters in length.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(6, ErrorMessage = "Postal code must be less than 7 characters in length.")]
        public string PostalCode { get; set; }

        [StringLength(30, ErrorMessage = "Email must be less than 31 characters in length.")]
        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Certification level is required")]
        [StringLength(1, ErrorMessage = "Certification level must be one character long.")]
        public string CertificationLevel { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }

        public virtual Certification Certification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
