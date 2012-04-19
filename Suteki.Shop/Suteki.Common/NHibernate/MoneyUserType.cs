using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using Suteki.Common.Models;

namespace Suteki.Common.NHibernate
{
    [Serializable]
    public class MoneyUserType : BaseImmutableUserType<Money>
    {
        public override object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var amount = ((decimal?)NHibernateUtil.Decimal.NullSafeGet(rs, names[0]));
            return amount.HasValue ? new Money(amount.Value) : Money.Zero;
        }

        public override void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var moneyObject = value as Money;
            object valueToSet;

            if (moneyObject != null)
            {
                valueToSet = moneyObject.Amount;
            }
            else
            {
                valueToSet = DBNull.Value;
            }

            NHibernateUtil.Decimal.NullSafeSet(cmd, valueToSet, index);
        }

        public override SqlType[] SqlTypes
        {
            get
            {
                return new[]
			       	{
			       		SqlTypeFactory.Decimal
			       	};
            }
        }
    }
}