﻿using Pazaak.Cards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Pazaak.Delegates;

namespace Pazaak.UserControls
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        /// <summary>
        /// Logic for when a new Match begins
        /// </summary>
        void BeginMatch(ObservableCollection<ICard> playerOneSideDeck, ObservableCollection<ICard> playerTwoSideDeck)
        {
            MainDeck = new Deck();
            MainDeck.InitializeAsMainDeck();

            playerOne = new Player("Player One", playerOneSideDeck);
            playerTwo = new Player("Player Two", playerTwoSideDeck);

            playerOne.Initialize(playerTwo, MainDeck, TurnTransition);
            playerTwo.Initialize(playerOne, MainDeck, TurnTransition);

            pctrlPlayerOne.DataContext = playerOne;
            pctrlPlayerOne.handControl.IsPlayCardAllowed = true;
            pctrlPlayerTwo.DataContext = playerTwo;
            pctrlPlayerOne.handControl.IsPlayCardAllowed = true;

            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());
            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());
            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());
            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());

            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
        }

        public void BeginMatchCustomDecks(Player playerOne, Player playerTwo)
        {
            MainDeck = new Deck();
            MainDeck.InitializeAsMainDeck();

            this.playerOne = playerOne;
            this.playerTwo = playerTwo;

            playerOne.Initialize(playerTwo, MainDeck, TurnTransition);
            playerTwo.Initialize(playerOne, MainDeck, TurnTransition);

            pctrlPlayerOne.DataContext = playerOne;
            pctrlPlayerOne.handControl.IsPlayCardAllowed = true;
            pctrlPlayerTwo.DataContext = playerTwo;
            pctrlPlayerOne.handControl.IsPlayCardAllowed = true;

            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());
            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());
            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());
            playerOne.Hand.Cards.Add(playerOne.SideDeck.DrawNextCard());

            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
            playerTwo.Hand.Cards.Add(playerTwo.SideDeck.DrawNextCard());
        }

        /// <summary>
        /// Logic for when a new round begins
        /// </summary>
        void BeginRound()
        {
            MainDeck.Shuffle();
            playerOne.SideDeck.Shuffle();
            playerTwo.SideDeck.Shuffle();

            playerOne.HasStood = false;
            playerOne.Board.Cards.Clear();
            playerOne.Board.UpdateSum();

            playerTwo.HasStood = false;
            playerTwo.Board.Cards.Clear();
            playerTwo.Board.UpdateSum();

            if (playerOneGoesFirst) { playerOne.BeginTurn(); }
            else { playerTwo.BeginTurn(); }
        }

        /// <summary>
        /// Creates a random SideDeck so we don't 
        /// have to pick out cards for it
        /// </summary>
        /// <returns> An ObservableCollection of randomly chosen Cards </returns>
        ObservableCollection<ICard> RandomizeSideDeck()
        {
            ObservableCollection<ICard> sideDeck = new ObservableCollection<ICard>();
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int cardType = rand.Next(6);
                int value;
                switch (cardType)
                {
                    case 0:
                        value = rand.Next(1, 9);
                        sideDeck.Add(new FlipValuesCard(new int[] { value, value + 2 }));
                        break;
                    case 1:
                        value = rand.Next(2, 4);
                        sideDeck.Add(new MultiplyLastCard(value));
                        break;
                    case 2:
                        value = rand.Next(1, 10);
                        sideDeck.Add(new MultiValueSignCard(new int[] { value, value + 1 }));
                        break;
                    case 3:
                        value = rand.Next(1, 10);
                        sideDeck.Add(new SignCard(value));
                        break;
                    case 4:
                        value = rand.Next(1, 10);
                        sideDeck.Add(new ValueCard(value));
                        break;
                    case 5:
                        sideDeck.Add(new MultiValueSignCard(new int[] { 1, 2 }) { IsTieBreaker = true });
                        break;
                }
            }
            return sideDeck;
        }

        /// <summary>
        /// Checks if a player has won the 
        /// round and/or the match and does the correct logic
        /// </summary>
        /// <param name="NextTurn"> Delegate for the next player's turn </param>
        void TurnTransition(NextPlayerBeginTurnDelegate NextTurn)
        {
            bool hasWon = false;
            if (playerOne.HasStood && playerTwo.HasStood || 
                playerOne.Board.Sum > 20 || playerTwo.Board.Sum > 20 || 
                playerOne.Board.Cards.Count >= 9 || playerTwo.Board.Cards.Count >= 9)
            {
                hasWon = Winchecks();
            }
            if (hasWon)
            {
                if (playerOne.Wins > 2)
                {
                    MessageBox.Show($"{playerOne.Name} won!", "Winner");

                    if (MessageBox.Show("Would you like to play again?", 
                        "Play Again", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BeginMatch(playerOne.SideDeck.Cards, playerTwo.SideDeck.Cards);
                        BeginRound();
                    }
                    else
                    {
                        MainWindow newWindow = new MainWindow();
                        newWindow.Show();

                        MainWindow oldWindow = (MainWindow)Window.GetWindow(this);
                        oldWindow.Close();
                        //Application.Current.Shutdown();
                    }
                }
                else if (playerTwo.Wins > 2)
                {
                    MessageBox.Show($"{playerTwo.Name} won!", "Winner");
                    if (MessageBox.Show("Would you like to play again?",
                        "Play Again", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BeginMatch(playerOne.SideDeck.Cards, playerTwo.SideDeck.Cards);
                        BeginRound();
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else { BeginRound(); }
            }
            else
            {
                NextTurn();
            }
        }

        /// <summary>
        /// Checks which player has won if any
        /// </summary>
        /// <returns></returns>
        private bool Winchecks()
        {
            bool won = false;
            if (playerOne.Board.Cards.Count >= 9 && playerOne.Board.Sum <= 20)
            {
                playerOne.Wins++;
                playerOneGoesFirst = true;
                won = true;
				MessageBox.Show("This round goes to Player One for filling their board without busting!");
            }
            else if (playerTwo.Board.Cards.Count >= 9 && playerTwo.Board.Sum <= 20)
            {
                playerTwo.Wins++;
                playerOneGoesFirst = false;
                won = true;
				MessageBox.Show("This round goes to Player Two for filling their board without busting!");
			}
			else if (playerOne.Board.Sum <= 19 && playerTwo.Board.Sum < playerOne.Board.Sum)
            {
                playerOne.Wins++;
                playerOneGoesFirst = true;
                won = true;
				MessageBox.Show("This round goes to Player One. They beat Player Two by " + (playerOne.Board.Sum - playerTwo.Board.Sum) + " points!");
			}
			else if (playerTwo.Board.Sum <= 19 && playerOne.Board.Sum < playerTwo.Board.Sum)
            {
                playerTwo.Wins++;
                playerOneGoesFirst = false;
                won = true;
				MessageBox.Show("This round goes to Player Two. They beat Player One by " + (playerTwo.Board.Sum - playerOne.Board.Sum) + " points!");
			}
			else if (playerOne.Board.Sum <= 19 && playerTwo.Board.Sum > 20)
            {
                playerOne.Wins++;
                playerOneGoesFirst = true;
                won = true;
				MessageBox.Show("This round goes to Player One. Player Two busted by " + (playerTwo.Board.Sum - 20) + " points!");
			}
			else if (playerTwo.Board.Sum <= 19 && playerOne.Board.Sum > 20)
            {
                playerTwo.Wins++;
                playerOneGoesFirst = false;
                won = true;
				MessageBox.Show("This round goes to Player Two. Player One busted by " + (playerOne.Board.Sum - 20) + " points!");
			}
			else if (playerOne.Board.Sum == 20 && playerTwo.Board.Sum != 20)
            {
                playerOne.Wins++;
                playerOneGoesFirst = true;
                won = true;
				MessageBox.Show("This round goes to Player One for getting exactly 20 points!");
			}
			else if (playerTwo.Board.Sum == 20 && playerOne.Board.Sum != 20)
            {
                playerTwo.Wins++;
                playerOneGoesFirst = false;
                won = true;
				MessageBox.Show("This round goes to Player Two for getting exactly 20 points!");
			}
			else if (playerOne.Board.Sum == playerTwo.Board.Sum)
            {
                if (playerOne.Board.GetTotalTieBreakerCards() > playerTwo.Board.GetTotalTieBreakerCards())
                {
                    playerOne.Wins++;
                    playerOneGoesFirst = true;
                    won = true;
					MessageBox.Show("This round goes to Player One for having more tiebreaker cards!");
				}
				else if (playerTwo.Board.GetTotalTieBreakerCards() > playerOne.Board.GetTotalTieBreakerCards())
                {
                    playerTwo.Wins++;
                    playerOneGoesFirst = false;
                    won = true;
					MessageBox.Show("This round goes to Player Two for having more tiebreaker cards!");
				}
				else
                {
                    won = true;
                    Random rand = new Random();
                    playerOneGoesFirst = rand.Next(2) == 0;
					MessageBox.Show("This round has resulted in a draw.");
				}
			}
			return won;
        }

        Player playerOne;
        Player playerTwo;
        private bool playerOneGoesFirst = true;
        private Deck mainDeck;
        public Deck MainDeck
        {
            get => mainDeck;
            set
            {
                mainDeck = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameControl()
        {
            InitializeComponent();

            ObservableCollection<ICard> sideDeck = RandomizeSideDeck();

            BeginMatch(sideDeck, sideDeck);

            BeginRound();
        }

        public GameControl(Player playerOne, Player playerTwo)
        {
            InitializeComponent();

            BeginMatchCustomDecks(playerOne, playerTwo);

            BeginRound();
        }
    }
}
