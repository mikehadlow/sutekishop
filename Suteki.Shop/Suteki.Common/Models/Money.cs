using System;

namespace Suteki.Common.Models
{
    public class Money
    {
        public decimal Amount { get; private set; }

        public Money(decimal amount)
        {
            Amount = amount;
        }

        public static string Symbol
        {
            get { return "£"; }
        }

        public static Money Zero
        {
            get { return new Money(0M);}
        }

        public override string ToString()
        {
            return Amount.ToString("0.00");
        }

        public string ToStringWithSymbol()
        {
            return Amount.ToString(Symbol + "0.00");
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherMoney = obj as Money;
            if (otherMoney == null) return false;
            return Amount.Equals(otherMoney.Amount);
        }

        public bool Equals(Money otherMoney)
        {
            if (otherMoney == null) return false;
            return Amount.Equals(otherMoney.Amount);
        }

        public static bool operator ==(Money a, Money b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Amount == b.Amount;
        }

        public static bool operator !=(Money a, Money b)
        {
            return !(a == b);
        }

        #region Arithmetic operator overloads

        public static Money operator +(Money a, Money b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a.Amount + b.Amount);
        }
        public static Money operator +(Money a, decimal b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            return new Money(a.Amount + b);
        }
        public static Money operator +(decimal a, Money b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a + b.Amount);
        }

        public static Money operator *(Money a, Money b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a.Amount * b.Amount);
        }
        public static Money operator *(Money a, decimal b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            return new Money(a.Amount * b);
        }
        public static Money operator *(decimal a, Money b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a * b.Amount);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a.Amount - b.Amount);
        }
        public static Money operator -(Money a, decimal b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            return new Money(a.Amount - b);
        }
        public static Money operator -(decimal a, Money b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a - b.Amount);
        }

        public static Money operator /(Money a, Money b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a.Amount / b.Amount);
        }
        public static Money operator /(Money a, decimal b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            return new Money(a.Amount / b);
        }
        public static Money operator /(decimal a, Money b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return new Money(a / b.Amount);
        }

        #endregion
    }
}