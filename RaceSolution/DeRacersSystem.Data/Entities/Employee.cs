namespace DeRacersSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Invoices = new HashSet<Invoice>();
            Orders = new HashSet<Order>();
            ReceiveOrders = new HashSet<ReceiveOrder>();
            SalesCartItems = new HashSet<SalesCartItem>();
        }

        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(30, ErrorMessage = "Last name must be less than 31 characters in length." )]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, ErrorMessage = "First name must be less than 31 characters in length.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(30, ErrorMessage = "Address must be less than 31 characters in length.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, ErrorMessage = "City length must be less than 31 characters in length.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(6, ErrorMessage = "Postal code must be 7 characters long.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(10, ErrorMessage = "Phone must be 11 characters in length.")]
        public string Phone { get; set; }

        public int PositionID { get; set; }

        [StringLength(50, ErrorMessage = "LoginId must be less than 51 characters in length.")]
        public string LoginId { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Social insurence number is required")]
        [StringLength(9, ErrorMessage = "Social insurence number must be less than 10 characters in length.")]
        public string SocialInsuranceNumber { get; set; }

        public virtual Position Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiveOrder> ReceiveOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesCartItem> SalesCartItems { get; set; }
    }
}
