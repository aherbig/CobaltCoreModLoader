﻿namespace DemoMod.Cards
{
    [CardMeta(deck = Deck.riggs, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EWandererDemoCard : Card
    {
        public override string Name() => "EWDemoCard";

        internal static Spr card_sprite = Spr.cards_GoatDrone;

        public override CardData GetData(State state) => new CardData
        {
            cost = 0,
            art = new Spr?(card_sprite)
        };

        public override void OnDraw(State s, Combat c)
        {
            var z = s.CharacterIsMissing(new Deck?(this.GetMeta().deck));
            var y = this.GetDataWithOverrides(s);
            var x = GetArt(s);

            base.OnDraw(s, c);
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ADrawCard() { count = 100 });
                    list.Add(new AAttack() { damage = 10, fast = true });

                    break;

                case Upgrade.A:
                    list.Add(new AHeal { healAmount = 100, targetPlayer = true });
                    break;

                case Upgrade.B:
                    list.Add(new ACorrodeDamage() { });
                    break;

                case (Upgrade)3:
                    list.Add(new ABubbleField() { all = true });
                    break;
            }

            return list;
        }
    }
}