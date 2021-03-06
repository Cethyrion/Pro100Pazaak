﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak.Cards
{
    public class SignCard: ValueCard
    {
        public void FlipSign()
        {
            Value *= -1;
        }

        override public ICard Copy()
        {
            return new SignCard(Value)
            {
                display = Display,
                IsTieBreaker = IsTieBreaker,
            };
        }

        public SignCard(int value) : base(value) { }
    }
}
