namespace Codecool.PlayingCards.Version1;

// public class Card
// {
//     public string Suit { get; }
//     public string Symbol { get; }
//     public string Title { get; }

//     public Card(string symbol, string suit)
//     {
//         Symbol = symbol;
//         Suit = suit;
//         Title = $"{Symbol} of {Suit}";
//     }

//     protected bool Equals(Card other)
//     {
//         return Suit == other.Suit && Symbol == other.Symbol;
//     }

//     public override bool Equals(object? obj)
//     {
//         if (ReferenceEquals(null, obj)) return false;
//         if (ReferenceEquals(this, obj)) return true;
//         if (obj.GetType() != this.GetType()) return false;
//         return Equals((Card)obj);
//     }

//     public override int GetHashCode()
//     {
//         return HashCode.Combine(Suit, Symbol);
//     }
//     public override string ToString()
//     {
//         return Title;
//     }
// }

//replace class Card with a Record
public record Card(string Symbol, string Suit);
// with old string display will look like this:
// public record Card(string Symbol, string Suit)
// {
//     public string Title => $"{Symbol} of {Suit}";

//     public override string ToString()
//     {
//         return Title;
//     }
// }

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
    //     // ILogger logger = new FileLogger("LogFile.txt");
    //     ILogger logger = new ConsoleLogger();
    //     ICardGenerator cardGenerator = new CardGenerator(logger);
    //     // IDeckBuilder frenchDeckBuilder = new FrenchDeckBuilder(cardGenerator);
    //     // IDeckBuilder germanDeckBuilder = new GermanDeckBuilder(cardGenerator);
    //     // IDeckBuilder[] builders = new IDeckBuilder[] { frenchDeckBuilder, germanDeckBuilder};

    //     // List<Deck> decks = BuildDecks(builders);
    //     // PrintCardCounts(decks);

    //     IDeckBuilder frenchDeckBuilder = new DeckBuilder(cardGenerator, DeckDescriptors.FrenchDeckDescriptor);

    //     Deck frenchDeck = frenchDeckBuilder.CreateDeck();
    //     Console.WriteLine(frenchDeck.CardCount);

    // }

    private static List<Deck> BuildDecks(IDeckBuilder[] builders)
    {
        List<Deck> decks = new List<Deck>();

        foreach (IDeckBuilder builder in builders)
        {
            decks.Add(builder.CreateDeck());
        }

        return decks;
    }

    private static void PrintCardCounts(List<Deck> decks)
    {
        foreach (Deck deck in decks)
        {
            Console.WriteLine(deck.CardCount);
        }
    }

}

// public class FrenchDeckBuilder : IDeckBuilder
// {
//     private static readonly int[] Numbers = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//     private static readonly string[] Symbols = { "Jack", "Queen", "King", "Ace" };
//     private static readonly string[] Suits = { "Clubs", "Spades", "Hearts", "Diamonds" };

//     private readonly ICardGenerator _cardGenerator;

//     public FrenchDeckBuilder(ICardGenerator cardGenerator)
//     {
//         _cardGenerator = cardGenerator;
//     }

//     public Deck CreateDeck()
//     {
//         var cards = _cardGenerator.Generate(Numbers, Symbols, Suits);
//         return new Deck(cards);
//     }
// }



// public class GermanDeckBuilder : IDeckBuilder
// {
//     private static readonly int[] Numbers = { 7, 8, 9, 10 };
//     private static readonly string[] Symbols = { "Unter", "Ober", "King", "Ace" };
//     private static readonly string[] Suits = { "Acorns", "Leaves", "Hearts", "Bells" };

//     private readonly ICardGenerator _cardGenerator;

//     public GermanDeckBuilder(ICardGenerator cardGenerator)
//     {
//         _cardGenerator = cardGenerator;
//     }

//     public Deck CreateDeck()
//     {
//         var cards = _cardGenerator.Generate(Numbers, Symbols, Suits);
//         return new Deck(cards);
//     }

// }

public class DeckBuilder : IDeckBuilder
{
    private readonly ICardGenerator _cardGenerator;
    private readonly DeckDescriptor _deckDescriptor;

    public DeckBuilder(ICardGenerator cardGenerator, DeckDescriptor deckDescriptor)
    {
        _deckDescriptor = deckDescriptor;
        _cardGenerator = cardGenerator;
    }

    public Deck CreateDeck()
    {
        var cards = _cardGenerator.Generate(_deckDescriptor);
        return new Deck(cards);
    }
}


public class CardGenerator : ICardGenerator
{
    private readonly ILogger _logger;

    public CardGenerator(ILogger logger)
    {
        _logger = logger;
    }
    public List<Card> Generate(DeckDescriptor deckDescriptor)
    {
        if (deckDescriptor == null) return new List<Card>();
        var iterator = GenerateCards(deckDescriptor.Numbers, deckDescriptor.Symbols, deckDescriptor.Suits).GetEnumerator();
        iterator.MoveNext();
        return iterator.Current;
    }

    private IEnumerable<List<Card>> GenerateCards(int[] numbers, string[] symbols, string[] suits)
    {
        List<Card> cards = new List<Card>();
        string[] numbersToString = Array.ConvertAll(numbers, x => x.ToString());
        string[] allNumbers = Enumerable.Concat(numbersToString, symbols).ToArray();
        foreach (var suit in suits)
        {
            AddCards(cards, suit, allNumbers);
        }

        yield return cards;
    }

    private void AddCards(List<Card> cards, string suit, string[] numbers)
    {
        foreach (var number in numbers)
        {
            Card card = new Card(number, suit);
            _logger.LogInfo($"Generated card {card}");
            cards.Add(card);
        }
    }
}

public interface ICardGenerator
{
    List<Card> Generate(DeckDescriptor deckDescriptor);
}

public interface IDeckBuilder
{
    Deck CreateDeck();
}

public interface ILogger
{
    public void LogInfo(string message);
    public void LogError(string message);
}

public class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        LogMessage(message, "INFO");
    }

    public void LogError(string message)
    {
        LogMessage(message, "ERROR");
    }

    private void LogMessage(string message, string type)
    {
        var entry = $"[{DateTime.Now}] {type}: {message}";
        Console.WriteLine(entry);
    }
}

public class FileLogger : ILogger
{
    private readonly string _logFile;

    public FileLogger(string logFile)
    {
        _logFile = logFile;
    }

    public void LogInfo(string message)
    {
        LogMessage(message, "INFO");
    }

    public void LogError(string message)
    {
        LogMessage(message, "ERROR");
    }

    private void LogMessage(string message, string type)
    {
        var entry = $"[{DateTime.Now}] {type}: {message}";
        using var streamWriter = File.AppendText(_logFile);
        streamWriter.WriteLine(entry);
    }
}

public record DeckDescriptor(int[] Numbers, string[] Symbols, string[] Suits);

public static class DeckDescriptors
{
    private static readonly int[] FrenchNumbers = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private static readonly string[] FrenchSymbols = { "Jack", "Queen", "King", "Ace" };
    private static readonly string[] FrenchSuits = { "Clubs", "Spades", "Hearts", "Diamonds" };

    private static readonly int[] GermanNumbers = { 7, 8, 9, 10 };
    private static readonly string[] GermanSymbols = { "Unter", "Ober", "King", "Ace" };
    private static readonly string[] GermanSuits = { "Clubs", "Spades", "Hearts", "Diamonds" };


    public static readonly DeckDescriptor FrenchDeckDescriptor = new DeckDescriptor(FrenchNumbers, FrenchSymbols, FrenchSuits);
    public static readonly DeckDescriptor GermanDeckDescriptor = new DeckDescriptor(GermanNumbers, GermanSymbols, GermanSuits);
}
