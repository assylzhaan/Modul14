using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul14HW
{
    public enum Mast
    {
        Hearts,
        Diamonds,
        Crosses,
        Spades
    }

    public enum Type
    {
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Prince,
        Queen,
        King,
        Ace
    }

    public class Karta
    {
        public Mast Mast { get; set; }
        public Type Type { get; set; }

        public Karta(Mast mast, Type tip)
        {
            Mast = mast;
            Type = tip;
        }
    }

    public class Player
    {
        public string Name { get; }
        public List<Karta> Koloda { get; }

        public Player(string name)
        {
            Name = name;
            Koloda = new List<Karta>();
        }

        public void AddKarta(Karta karta)
        {
            Koloda.Add(karta);
        }

        public void ShowKoloda()
        {
            Console.WriteLine($"{Name}'s koloda:");
            foreach (var karta in Koloda)
            {
                Console.WriteLine($"{karta.Type} of {karta.Mast}");
            }
        }
    }

    public class Game
    {
        private List<Player> players;
        private List<Karta> koloda;

        public Game(List<string> playersNames)
        {
            players = playersNames.Select(name => new Player(name)).ToList();
            koloda = GenerateKoloda();
            ShuffleKoloda();
            DistributeKarty();
        }

        private List<Karta> GenerateKoloda()
        {
            var koloda = new List<Karta>();
            foreach (Mast mast in Enum.GetValues(typeof(Mast)))
            {
                foreach (Type tip in Enum.GetValues(typeof(Type)))
                {
                    koloda.Add(new Karta(mast, tip));
                }
            }
            return koloda;
        }

        private void ShuffleKoloda()
        {
            Random random = new Random();
            koloda = koloda.OrderBy(x => random.Next()).ToList();
        }

        private void DistributeKarty()
        {
            int playersCount = players.Count;
            int cardsPerPlayer = koloda.Count / playersCount;

            for (int i = 0; i < playersCount; i++)
            {
                players[i].Koloda.AddRange(koloda.Skip(i * cardsPerPlayer).Take(cardsPerPlayer));
            }
        }

        public void Play()
        {
            while (!IsGameOver())
            {
                PlayRound();
            }

            Player winner = players.OrderByDescending(player => player.Koloda.Count).First();
            Console.WriteLine($"{winner.Name} winner!");
        }

        private void PlayRound()
        {
            List<Karta> roundKarty = players.Select(player => player.Koloda.First()).ToList();
            Karta maxKarta = roundKarty.OrderByDescending(karta => KartaValue(karta)).First();
            int winnerIndex = roundKarty.FindIndex(karta => karta == maxKarta);
            players[winnerIndex].Koloda.AddRange(roundKarty);

            foreach (var player in players)
            {
                player.Koloda.RemoveAt(0);
            }
        }

        private int KartaValue(Karta karta)
        {
            Dictionary<Type, int> tipValues = new Dictionary<Type, int>
        {
            { Type.Six, 6 },
            { Type.Seven, 7 },
            { Type.Eight, 8 },
            { Type.Nine, 9 },
            { Type.Ten, 10 },
            { Type.Prince, 11 },
            { Type.Queen, 12 },
            { Type.King, 13 },
            { Type.Ace, 14 }
        };

            return tipValues[karta.Type];
        }

        private bool IsGameOver()
        {
            return players.Any(player => player.Koloda.Count == 0);
        }
    }

    class Program
    {
        static void Main()
        {
            List<string> playersNames = new List<string> { "Player1", "Player2" };
            Game game = new Game(playersNames);
            game.Play();
        }
    }

}
