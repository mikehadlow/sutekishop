using System.Collections.Generic;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class CardType : INamedEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool RequiredIssueNumber { get; set; }

        IList<Card> cards = new List<Card>();
        public virtual IList<Card> Cards
        {
            get { return cards; }
            set { cards = value; }
        }

        // these constants should match database table CardType.CardTypeId
        public const int VisaDeltaElectronId = 1;
        public const int MasterCardEuroCardId = 2;
        public const int AmericanExpressId = 3;
        public const int SwitchSoloMaestroId = 4;

        public static CardType VisaDeltaElectron
        {
            get { return new CardType {Id = VisaDeltaElectronId}; }
        }
        public static CardType MasterCardEuroCard
        {
            get { return new CardType { Id = MasterCardEuroCardId }; }
        }
        public static CardType AmericanExpress
        {
            get { return new CardType { Id = AmericanExpressId }; }
        }
        public static CardType SwitchSoloMaestro
        {
            get { return new CardType { Id = SwitchSoloMaestroId }; }
        }
    }
}
