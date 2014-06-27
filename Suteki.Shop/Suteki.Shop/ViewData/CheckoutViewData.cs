using System.ComponentModel.DataAnnotations;
using Suteki.Common.ViewData;
using Suteki.Shop.Models.CustomDataAnnotations;

namespace Suteki.Shop.ViewData
{
    public class CheckoutViewData : ViewDataBase
    {
        public int OrderId { get; set; }
        public int BasketId { get; set; }

        // card contact

        public string CardContactFirstName { get; set; }
        public string CardContactLastName { get; set; }
        public string CardContactAddress1 { get; set; }
        public string CardContactAddress2 { get; set; }
        public string CardContactAddress3 { get; set; }
        public string CardContactTown { get; set; }
        public string CardContactCounty { get; set; }
        public string CardContactPostcode { get; set; }
        public Country CardContactCountry { get; set; }
        public string CardContactTelephone { get; set; }

        // email

        [Required(ErrorMessage = "Email is required")]
        [Email(ErrorMessage = "Not a valid email address")]
        [StringLength(250, ErrorMessage = "Email must not be longer than 250 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirm Email is required")]
        [Email(ErrorMessage = "Not a valid email address")]
        [StringLength(250, ErrorMessage = "Email must not be longer than 250 characters")]
        public string EmailConfirm { get; set; }

        // delivery contact

        public bool UseCardholderContact { get; set; }

        public string DeliveryContactFirstName { get; set; }
        public string DeliveryContactLastName { get; set; }
        public string DeliveryContactAddress1 { get; set; }
        public string DeliveryContactAddress2 { get; set; }
        public string DeliveryContactAddress3 { get; set; }
        public string DeliveryContactTown { get; set; }
        public string DeliveryContactCounty { get; set; }
        public string DeliveryContactPostcode { get; set; }
        public Country DeliveryContactCountry { get; set; }
        public string DeliveryContactTelephone { get; set; }

        // additional

        public string AdditionalInformation { get; set; }

        // card details

        public CardType CardCardType { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public int CardStartMonth { get; set; }
        public int CardStartYear { get; set; }
        public string CardIssueNumber { get; set; }
        public string CardSecurityCode { get; set; }

        public bool PayByTelephone { get; set; }

        // contact agreement

        public bool ContactMe { get; set; }

        // Where did you hear from us?

        public Referer Referer { get; set; }
    }
}