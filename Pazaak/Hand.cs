﻿using Pazaak.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Pazaak
{
    class Hand
    {
        private ObservableCollection<ICard> cards;

        public ObservableCollection<ICard> Cards { get => cards; set { cards = value; } }

		/// <summary>
		/// 
		/// Calls the chosen card's DoCardEffect 
		/// then adds a value card with the chosen 
		/// card's value
		/// 
		/// </summary>
		/// <param name="cardIndex"> index of the chosen card in the member ICard array Cards </param>
		/// <param name="board"> The board that the new card will be added to </param>

        public Hand()
        {
            cards = new ObservableCollection<ICard>();
        }
    }
}
