using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using Suteki.Shop.Models.CustomDataAnnotations;

namespace Suteki.Shop
{
    public class Card : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "You must enter a value for Card Holder")]
        [StringLength(50, ErrorMessage = "Card Holder cannot be more than 50 characters long")]
        public virtual string Holder { get; set; }

        [Required(ErrorMessage = "You must enter a value for Card Number")]
        [StringLength(19, ErrorMessage = "A credit card number can not be more than 19 characters long")]
        [CreditCard(ErrorMessage = "Not a valid credit card number")]
        public virtual string Number { get; set; }

        public virtual int ExpiryMonth { get; set; }
        public virtual int ExpiryYear { get; set; }
        public virtual int StartMonth { get; set; }
        public virtual int StartYear { get; set; }

        [Numeric(ErrorMessage = "Issue Number may only be a number")]
        [StringLength(1, ErrorMessage = "Issue Number may only be one character")]
        public virtual string IssueNumber { get; set; }

        [Required(ErrorMessage = "Security Code is required")]
        [Numeric(ErrorMessage = "SecurityCode must be a number")]
        [StringLength(4, ErrorMessage = "Security Code must not be more than 4 characters long")]
        public virtual string SecurityCode { get; set; }

        public virtual CardType CardType { get; set; }

        IList<Order> orders = new List<Order>();
        public virtual IList<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        public static IEnumerable<int> Months
        {
            get
            {
                return 1.To(12);
            }
        }

        public static IEnumerable<int> ExpiryYears
        {
            get
            {
                return DateTime.Now.Year.To(DateTime.Now.Year + 8);
            }
        }

        public static IEnumerable<int> StartYears
        {
            get
            {
                return (DateTime.Now.Year - 4).To(DateTime.Now.Year);
            }
        }

        public virtual Card Copy()
        {
            return new Card
            {
                CardType = CardType,
                Holder = Holder,
                Number = Number,
                IssueNumber = IssueNumber,
                SecurityCode = SecurityCode,
                StartMonth = StartMonth,
                StartYear = StartYear,
                ExpiryMonth = ExpiryMonth,
                ExpiryYear = ExpiryYear
            };
        }

        // encrypted value setters

        public virtual void SetEncryptedNumber(string number)
        {
            this.Number = number;
        }

        public virtual void SetEncryptedSecurityCode(string securityCode)
        {
            this.SecurityCode = securityCode;
        }

        // computed properties

        public virtual string StartDateAsString
        {
            get
            {
                if(StartMonth == 0 || StartYear == 0)
                {
                    return "&nbsp;";
                }
                return "{0:00} / {1:0000}".With(StartMonth, StartYear);
            }
        }

        public virtual string ExpiryDateAsString
        {
            get
            {
                return "{0:00} / {1:0000}".With(ExpiryMonth, ExpiryYear);
            }
        }

        public virtual string CardNumberAsString
        {
            get
            {
                var trimmedValue = Regex.Replace(Number, "[^0-9]", "");
                return Regex.Replace(trimmedValue, @"^(\d{4})(\d{4})(\d{4})(\d{1,4})(\d{0,4})$", @"$1 $2 $3 $4 $5");
            }
        }
    }
}
