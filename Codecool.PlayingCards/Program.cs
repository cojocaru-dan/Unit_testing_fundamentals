namespace Codecool.PlayingCards.Model;

public enum FrenchSuit 
{
    Diamonds,
    Clubs,
    Hearts,
    Spades,
}
public enum GermanSuit
{
    Acorns,
    Leaves,
    Hearts,
    Bells,
}
// mutable Card class
/* public class Card 
{

public Suit Suit { get; set; }
public string Symbol { get; set; }

//The IDE will signal that this constructor is redundant
public Card(string symbol, Suit suit)
{
    Symbol = symbol;
    Suit = suit;
}

public static void Main()
{
    Card card = new Card("Ace", Suit.Spades);
    Console.WriteLine($"{card.Symbol} of {card.Suit}");
    Console.ReadKey(); // We can add this line to stop the debugger from immediately exiting after the execution. Instead, it will wait for a keystroke to exit.

}
} */

// immutable Card class
/* public class Card 
{
public string Symbol { get; init; }
public Suit Suit { get; init; }

public Card(string symbol, Suit suit)
{
    Symbol = symbol;
    Suit = suit;
}
public static void Main()
{
    Card card = new Card("Ace", Suit.Spades);
    Console.WriteLine($"{card.Symbol} of {card.Suit}");
    Console.ReadKey(); // We can add this line to stop the debugger from immediately exiting after the execution. Instead, it will wait for a keystroke to exit.

}
} */
// you can remove init; and you will have get-only properties
public class Card
{
    public string Suit { get; }
    public string Symbol { get; }
    public string Title { get; }

    public Card(string symbol, string suit)
    {
        Symbol = symbol;
        Suit = suit;
        Title = $"{Symbol} of {Suit}";
    }

    protected bool Equals(Card other)
    {
        return Suit == other.Suit && Symbol == other.Symbol;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Card)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Suit, Symbol);
    }
    public override string ToString()
    {
        return Title;
    }
}

public class FrenchCard
{
    public FrenchSuit Suit { get; }
    public string Symbol { get; }
    public string Title { get; }

    public FrenchCard(string symbol, FrenchSuit suit)
    {
        Symbol = symbol;
        Suit = suit;
        Title = $"{Symbol} of {Suit}";
    }

    protected bool Equals(FrenchCard other)
    {
        return Suit == other.Suit && Symbol == other.Symbol;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FrenchCard)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Suit, Symbol);
    }

    public override string ToString()
    {
        return Title;
    }
}

public class GermanCard
{
    public GermanSuit Suit { get; }
    public string Symbol { get; }
    public string Title { get; }

    public GermanCard(string symbol, GermanSuit suit)
    {
        Symbol = symbol;
        Suit = suit;
        Title = $"{Symbol} of {Suit}";
    }

    protected bool Equals(GermanCard other)
    {
        return Suit == other.Suit && Symbol == other.Symbol;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GermanCard)obj);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine((int)Suit, Symbol);
    }
    public override string ToString()
    {
        return Title;
    }
}

public class Deck
{
    private static readonly Random Random = new Random();

    private readonly List<Card> _cards;
    private readonly List<Card> _drawn;

    public int CardCount => _cards.Count;

    public Deck(List<Card> cards)
    {
        _cards = cards;
        _drawn = new List<Card>();
    }

    public Card? DrawOne()
    {
        if (CardCount == 0) return null;
        Card card = _cards[Random.Next(0, _cards.Count - 1)];
        HandleDraw(card);
        return card;
    }

    private void HandleDraw(Card card)
    {
        _cards.Remove(card);
        _drawn.Add(card);
    }

    public void Reset()
    {
        List<Card> current = new List<Card>(_cards);
        _cards.Clear();
        _cards.AddRange(current.Concat(_drawn));
    }
}
internal class PlayingCards
{
    // public static void Main(string[] args)
    // {
    //     Deck frenchDeck = GenerateFrenchDeck();
    //     Console.WriteLine($"French deck created. Card count: {frenchDeck.CardCount}"); // 52

    //     Card? card = frenchDeck.DrawOne();
    //     Console.WriteLine($"{card} was drawn. Card count: {frenchDeck.CardCount}"); //51

    //     frenchDeck.Reset();
    //     Console.WriteLine($"Deck has been reset. Card count: {frenchDeck.CardCount}"); //52

    //     Console.ReadKey();
    // }

    private static Deck GenerateFrenchDeck()
    {
        int[] numbers = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        string[] symbols = { "Jack", "Queen", "King", "Ace" };
        string[] suits = { "Clubs", "Spades", "Hearts", "Diamonds" };

        return GenerateDeck(numbers, symbols, suits);
    }

    private static Deck GenerateGermanDeck()
    {
        int[] numbers = { 7, 8, 9, 10 };
        string[] symbols = { "Unter", "Ober", "King", "Ace" };
        string[] suits = { "Acorns", "Leaves", "Hearts", "Bells" };

        return GenerateDeck(numbers, symbols, suits);
    }

    private static Deck GenerateDeck(int[] numbers, string[] symbols, string[] suits)
    {
        List<Card> cards = new List<Card>();

        foreach (var suit in suits)
        {
            AddNumberedCards(cards, numbers, suit);
            AddCourtCards(cards, symbols, suit);
        }

        return new Deck(cards);
    }

    private static void AddNumberedCards(List<Card> cards, int[] numbers, string suit)
    {
        foreach (var number in numbers)
        {
            Card card = new Card(number.ToString(), suit);
            cards.Add(card);
        }
    }

    private static void AddCourtCards(List<Card> cards, string[] symbols, string suit)
    {
        foreach (var symbol in symbols)
        {
            Card card = new Card(symbol, suit);
            cards.Add(card);
        }
    }

    // private static void PrintDeck(Card[] deck)
    // {
    //     for (int i = 0; i < deck.Length; i++)
    //     {
    //         Console.WriteLine($"{i + 1} - {deck[i]}");
    //     }
    // }
}
