﻿using Pazaak.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak.Cards
{
    class FlipValuesCard : ValueCard
    {
        private string CreateValuesString()
        {
            string allValues = "";

            for (int i = 0; i < flipValues.Length; i++)
            {
                allValues += $"{flipValues[i]}&";
            }

            return allValues.Remove(allValues.Length - 1);
        }

        public override void DoCardEffect(Board board)
        {
            foreach (ValueCard card in board.Cards)
            {
                foreach (int flipVal in flipValues)
                {
                    if (Math.Abs(card.Value) == flipVal)
                    {
                        card.Value *= -1;
                    }
                }
            }
        }

        public override ICard Copy()
        {
            return new FlipValuesCard(FlipValues)
            {
                value = Value,
                display = Display,
                IsTieBreaker = IsTieBreaker,
            };
        }

        protected int[] flipValues;

        public override int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                FieldChanged("Value");
            }
        }

        public int[] FlipValues
        {
            get
            {
                return flipValues;
            }
            set
            {
                flipValues = value.RemoveDuplicatesAndMakePositive();

                Display = CreateValuesString();
                FieldChanged("Value");
            }
        }

        public FlipValuesCard(int[] flipValues) : base(0)
        {
            FlipValues = flipValues;
        }
    }
}
