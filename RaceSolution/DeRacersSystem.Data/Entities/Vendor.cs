namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            Orders = new HashSet<Order>();
            VendorCatalogs = new HashSet<VendorCatalog>();
        }

        public int VendorID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(30)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30)]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(6)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorCatalog> VendorCatalogs { get; set; }
    }
}
