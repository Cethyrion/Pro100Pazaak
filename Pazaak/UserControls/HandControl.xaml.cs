﻿using Pazaak.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pazaak.UserControls
{
    /// <summary>
    /// Interaction logic for HandControl.xaml
    /// </summary>
    public partial class HandControl : UserControl
    {
        Binding IsActive;

        public HandControl()
        {
            InitializeComponent();

            IsActive = new Binding("IsActive")
            {
                Source = this.DataContext
            };
        }

		private void CardControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Player thisPlayer = (Player)this.DataContext;
			ICard thisCard = (ICard)((FrameworkElement)sender).DataContext;

			thisPlayer.Board.AddCard(thisCard);
            thisPlayer.Hand.Cards.Remove(thisCard);
		}
	}
}
