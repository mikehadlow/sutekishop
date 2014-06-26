using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Suteki.Common.Events;
using Suteki.Common.Models;
using Suteki.Shop.Events;
using Suteki.Shop.Models.CustomDataAnnotations;
using Suteki.Common.Extensions;

namespace Suteki.Shop
{
    public class Order : IEntity, IAmOwnedBy
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Email(ErrorMessage = "Not a valid email address")]
        [StringLength(250, ErrorMessage = "Email must not be longer than 250 characters")]
        public virtual string Email { get; set; }

        public virtual string AdditionalInformation { get; set; }
        public virtual bool UseCardHolderContact { get; set; }
        public virtual bool PayByTelephone { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime DispatchedDate { get; set; }

        [StringLength(1000, ErrorMessage = "Note must not be longer than 1000 characters")]
        public virtual string Note { get; set; }

        public virtual bool ContactMe { get; set; }
        public virtual Card Card { get; set; }
        public virtual Contact CardContact { get; set; }
        public virtual Contact DeliveryContact { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual User User { get; set; }
        public virtual User ModifiedBy { get; set; }

        [StringLength(250, ErrorMessage = "Tracking number must not be longer than 250 characters")]
        public virtual string TrackingNumber { get; set; }

        public virtual bool Problem { get; set; }

        private IList<OrderLine> orderLines = new List<OrderLine>();
        public virtual IList<OrderLine> OrderLines
        {
            get { return orderLines; }
            protected set { orderLines = value; }
        }

        private IList<OrderAdjustment> adjustments = new List<OrderAdjustment>();
        public virtual IList<OrderAdjustment> Adjustments
        {
            get { return adjustments; }
            protected set { adjustments = value; }
        }

        public virtual Contact PostalContact
        {
            get
            {
                return UseCardHolderContact ? CardContact : DeliveryContact;
            }
        }

        public virtual string DispatchedDateAsString
        {
            get
            {
                if (IsDispatched) return DispatchedDate.ToShortDateString();
                return "&nbsp;";
            }
        }

        public virtual string UserAsString
        {
            get
            {
                if (ModifiedBy != null)
                {
                    return ModifiedBy.Email;
                }
                return "&nbsp;";
            }
        }

        public virtual bool IsCreated { get { return OrderStatus.Id == OrderStatus.CreatedId; } }
        public virtual bool IsDispatched { get { return OrderStatus.Id == OrderStatus.DispatchedId; } }
        public virtual bool IsRejected { get { return OrderStatus.Id == OrderStatus.RejectedId; } }

        public virtual Money Total
        {
            get 
            { 
                return orderLines.Select(line => line.Total)
                    .Concat(adjustments.Select(a => a.Amount))
                    .Sum(); 
            }
        }

        public virtual PostageResult Postage { get; set; }

        public virtual string PostageTotal
        {
            get
            {
                if (Postage == null) return " - ";
                if (Postage.Phone) return "Phone";
                return Postage.Price.ToStringWithSymbol();
            }
        }

        public virtual string TotalWithPostage
        {
            get
            {
                if (Postage == null) return " - ";
                if (Postage.Phone) return "Phone";
                return (Postage.Price + Total).ToStringWithSymbol();
            }
        }

        public virtual string PostageDescription
        {
            get
            {
                if (Postage == null) return "No postage calculated";
                return Postage.Description;
            }
        }

        public virtual void AddLine(string productName, int quantity, Money price, string productUrlName, int productId, string sizeName)
        {
            if (productName == null)
            {
                throw new ArgumentNullException("productName");
            }
            if (quantity == 0)
            {
                throw new ArgumentException("quantity can not be zero");
            }
            if (productUrlName == null)
            {
                throw new ArgumentNullException("productUrlName");
            }
            if (sizeName == null)
            {
                throw new ArgumentNullException("sizeName");
            }

            var orderLine = new OrderLine
            {
                ProductName = productName,
                Quantity = quantity,
                Price = price,
                Order = this,
                ProductUrlName = productUrlName,
                ProductId = productId,
                SizeName = sizeName
            };

            OrderLines.Add(orderLine);
        }

        public virtual void Confirm()
        {
            if (OrderStatus.Id != OrderStatus.PendingId)
            {
                throw new InvalidOperationException("Can only confirm when the order status is pending");
            }

            OrderStatus = OrderStatus.Created;
            DomainEvent.Raise(new OrderConfirmed(this));
        }

        public virtual void Dispatch(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (OrderStatus.Id != OrderStatus.CreatedId)
            {
                throw new InvalidOperationException("Can only dispatch when the order status is Created");
            }

            OrderStatus = OrderStatus.Dispatched;
            DispatchedDate = DateTime.Now;
            ModifiedBy = user;
            DomainEvent.Raise(new OrderDispatched(this));
        }

        public virtual void Reject(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (OrderStatus.Id != OrderStatus.CreatedId)
            {
                throw new InvalidOperationException("Can only reject when order status is Created");
            }

            OrderStatus = OrderStatus.Rejected;
            ModifiedBy = user;
        }

        public virtual void ResetStatus()
        {
            if (!(OrderStatus.Id == OrderStatus.DispatchedId || OrderStatus.Id == OrderStatus.RejectedId))
            {
                throw new InvalidOperationException("Can only reset status when order status is dispatched or created");
            }
            OrderStatus = OrderStatus.Created;
            ModifiedBy = null;
        }

        public virtual void AddAdjustment(OrderAdjustment adjustment)
        {
            if (adjustment == null)
            {
                throw new ArgumentNullException("adjustment");
            }

            adjustment.Order = this;
            adjustments.Add(adjustment);
        }

        public virtual void RemoveAdjustment(OrderAdjustment adjustment)
        {
            if (adjustment == null)
            {
                throw new ArgumentNullException("adjustment");
            }

            adjustment.Order = null;
            adjustments.Remove(adjustment);
        }
    }
}
