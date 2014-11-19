using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfWar
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck TheDeck = new Deck();
            TheDeck.Shuffle();
            TheDeck.Shuffle();

            WarPlayer player1 = new WarPlayer();
            WarPlayer player2 = new WarPlayer();

            // Deal cards
            while (TheDeck.GetNumOfCards() > 0)
            {
                if (TheDeck.GetNumOfCards() % 2 == 0)
                {
                    player1.takeCard(TheDeck.DealACard());
                }
                else
                {
                    player2.takeCard(TheDeck.DealACard());
                }
            }

            List<Card> roundPot = new List<Card>();
            Console.WriteLine("Welcome to the Game of War");
            // For each turn
            while (true)
            {
                if (HasSomeoneWon(player1, player2)) { break; }
                // Take 3 cards from each player and put it in the pot
                for (int i = 0; i < 3; i++)
                {

                    if (player1.Hand.Count > 1)
                    {
                        roundPot.Add(player1.giveCard());
                    }
                    if (player2.Hand.Count > 1)
                    {
                        roundPot.Add(player2.giveCard());
                    }
                }

                // Now compare their two cards to determine a winner
                Card player1Card = player1.giveCard();
                Card player2Card = player2.giveCard();

                if (player1Card.CompareTo(player2Card) == 1)
                {
                    // player 1 wins
                    player1.takeCard(player1Card);
                    player1.takeCard(player2Card);
                    foreach (Card item in roundPot)
                    {
                        player1.takeCard(item);
                    }
                    roundPot.Clear();
                    DisplayRoundInfo("Player 1", player1, player2, player1Card, player2Card);
                }
                else if (player1Card.CompareTo(player2Card) == -1)
                {
                    // player 2 wins
                    player2.takeCard(player1Card);
                    player2.takeCard(player2Card);
                    foreach (Card item in roundPot)
                    {
                        player2.takeCard(item);
                    }
                    roundPot.Clear();
                    DisplayRoundInfo("Player 2", player1, player2, player1Card,player2Card);
                }
                else
                {

                    if (HasSomeoneWon(player1, player2)) { break; }
                    Console.WriteLine("Tie Happened");
                    // Tie
                    // add both player cards to the pot
                    roundPot.Add(player1Card);
                    roundPot.Add(player2Card);

                }
            }

            // see if someone has run out of cards
            if (player1.Hand.Count() > 0)
            {
                Console.WriteLine("Player 1 wins");
            }
            else
            {
                Console.WriteLine("Player 2 wins");
            }
            Console.WriteLine("Game Over");
            Console.ReadKey();

        }

        public static void DisplayRoundInfo(string roundWinner, WarPlayer p1, WarPlayer p2, Card p1c, Card p2c)
        {
            Console.WriteLine("Round Ended, {0} won that round. P1: {1}, P2: {2}", roundWinner,p1c,p2c);
            Console.WriteLine("Player 1 has {0} cards left, Player 2 has {1} cards left", p1.Hand.Count(), p2.Hand.Count());
            System.Threading.Thread.Sleep(1200);
        }

        public static bool HasSomeoneWon(WarPlayer p1, WarPlayer p2)
        {
            if (p1.Hand.Count() == 0 || p2.Hand.Count() == 0)
            { return true; }
            else { return false; }
        }
    }

    class WarPlayer
    {
        public List<Card> Hand = new List<Card>();
        public void takeCard(Card newCard)
        {
            Hand.Add(newCard);
        }
        public Card giveCard()
        {
            Card tempCard = Hand[0];
            Hand.RemoveAt(0);
            return tempCard;

        }
    }
    class Card
    {
        public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }
        public enum Suit { Hearts, Spades, Dimonds, Clubs }

        public Rank MyRank { get; set; }
        public Suit MySuit { get; set; }

        public override string ToString()
        {
            return (this.MyRank.ToString() + " of " + this.MySuit.ToString());
        }

        public Card(int suit, int rank)
        {
            this.MySuit = (Suit)suit;
            this.MyRank = (Rank)rank;
        }

        public int CompareTo(object obj)
        {
            Card secondCard = (Card)obj;
            if (this.MyRank > secondCard.MyRank)
            {
                return 1;
            }
            if (this.MyRank < secondCard.MyRank)
            {
                return -1;
            }
            /*
            if (this.MySuit > secondCard.MySuit)
            {
                return 1;
            }
            if (this.MySuit < secondCard.MySuit)
            {
                return -1;
            }*/
            return 0;
        }
    }

    /// <summary>
    /// A class that holds Cards and features a few helper methods
    /// </summary>
    class Deck
    {
        private List<Card> unusedCards = new List<Card>();
        private List<Card> dealtCards = new List<Card>();
        private Random rng = new Random();

        /// <summary>
        /// Constructor that initializes the deck of cards
        /// </summary>
        public Deck()
        {
            // Fill the unusedCards with a set of cards

            // Suits first
            for (int i = 0; i < 4; i++)
            {
                // Cards, which start with 2 and go till Ace (14)
                for (int j = 2; j < 15; j++)
                {
                    // Add this new card to the deck
                    unusedCards.Add(new Card(i,j));
                }
            }
        }
        /// <summary>
        /// Returns a list of 5 cards to make up a hand for the PokerPlayer
        /// </summary>
        /// <returns>list of 5 cards</returns>
        public List<Card> Deal5Cards()
        {
            // Return 5 cards to the PokerPlayer
            List<Card> tempHand = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                tempHand.Add(this.DealACard());
            }
            return tempHand;
        }
        /// <summary>
        /// Picks a random card then removes it from the unused list and adds it to the dealt list
        /// </summary>
        /// <returns>the random card to a player/hand</returns>
        public Card DealACard()
        {
            // Get a random card from the unused deck
            int randomIndex = rng.Next(GetNumOfCards());
            Card tempCard = unusedCards.ElementAt(randomIndex);
            //Put the card in the dealt deck
            dealtCards.Add(tempCard);
            // Remove card from the unused deck
            unusedCards.RemoveAt(randomIndex);

            // Return that card to the player/hand
            return tempCard;
        }

        /// <summary>
        /// Uses in list shuffling to reorder the existing cards
        /// </summary>
        public void Shuffle()
        {
            // When there is more then 1 card to shuffle
            if (unusedCards.Count > 1)
            {
                // Go through each index of the list
                for (int i = unusedCards.Count - 1; i >= 0; i--)
                {
                    // Pick a random card to swap with this index
                    Card tmp = unusedCards[i];
                    int randomIndex = rng.Next(i + 1);

                    //Swap elements
                    unusedCards[i] = unusedCards[randomIndex];
                    unusedCards[randomIndex] = tmp;
                }
            }
        }
        /// <summary>
        /// Returns the number of unused cards in the deck
        /// </summary>
        /// <returns>number of unused cards in the deck</returns>
        public int GetNumOfCards()
        {
            return unusedCards.Count();
        }
    }
}
