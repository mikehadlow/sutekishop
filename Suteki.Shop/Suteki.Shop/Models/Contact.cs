using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Suteki.Common.Extensions;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Contact : IEntity
    {
        public Contact()
        {
            // initialise non-required properties to empty strings
            Address2 = "";
            Address3 = "";
            Town = "";
            County = "";
            Postcode = "";
            Telephone = "";
        }

        public virtual int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name must not be longer than 50 characters")]
        public virtual string Firstname { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name must not be longer than 50 characters")]
        public virtual string Lastname { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required")]
        [StringLength(100, ErrorMessage = "Address Line 1 must not be longer than 100 characters")]
        public virtual string Address1 { get; set; }

        [StringLength(100, ErrorMessage = "Address Line 2 must not be longer than 100 characters")]
        public virtual string Address2 { get; set; }

        [StringLength(100, ErrorMessage = "Address Line 3 must not be longer than 100 characters")]
        public virtual string Address3 { get; set; }

        [StringLength(50, ErrorMessage = "Town must not be longer than 50 characters")]
        public virtual string Town { get; set; }

        [StringLength(50, ErrorMessage = "County must not be longer than 50 characters")]
        public virtual string County { get; set; }

        [StringLength(50, ErrorMessage = "Postcode must not be longer than 50 characters")]
        public virtual string Postcode { get; set; }

        [StringLength(50, ErrorMessage = "Telephone must not be longer than 50 characters")]
        public virtual string Telephone { get; set; }

        public virtual Country Country { get; set; }

        public virtual string Fullname
        {
            get
            {
                return "{0} {1}".With(Firstname, Lastname);
            }
        }

        /// <summary>
        /// Get an enumerator over the contact lines, but only return the ones that are available.
        /// This is a convenience method that allows us to easily write a nice address by simply
        /// foreach(ing) over GetAddressLines:
        /// 
        /// foreach(var line in myContact.GetAddressLines())
        /// {
        ///     Console.WriteLine(line);
        /// }
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetAddressLines()
        {
            yield return Fullname;
            yield return Address1;

            if (!string.IsNullOrEmpty(Address2))
            {
                yield return Address2;
            }

            if (!string.IsNullOrEmpty(Address3))
            {
                yield return Address3;
            }

            if (!string.IsNullOrEmpty(Town))
            {
                yield return Town;
            }

            if (!string.IsNullOrEmpty(County))
            {
                yield return County;
            }

            if (!string.IsNullOrEmpty(Postcode))
            {
                yield return Postcode;
            }

            yield return Country.Name;
        }
    }
}
