using Codecool.PlayingCards.Version1;
using Moq;
namespace UnitTests;
// public class CardGeneratorTest
// {
//     private static ILogger _logger = new ConsoleLogger();
//     private static ICardGenerator _cardGenerator = new CardGenerator(_logger);
//     private static int[] _numbers = new[] { 2, 3, 4 };
//     private static string[] _symbols = new[] { "J", "Q", "K" };
//     private static string[] _suits = new[] { "Hearts", "Diamonds" };
//     private static DeckDescriptor _deckDescriptor = new DeckDescriptor(_numbers, _symbols, _suits);
//     [Test]
//     public void GenerateCardsReturnsExpectedNumberOfCards()
//     {
//         // Arrange
//         var expectedCardCount = _deckDescriptor.Numbers.Length * _deckDescriptor.Symbols.Length * _deckDescriptor.Suits.Length;

//         // Act
//         var cards = _cardGenerator.Generate(_deckDescriptor);

//         // Assert
//         Assert.That(cards.Count, Is.EqualTo(expectedCardCount));
//     }
//     [Test]
//     public void GenerateCardsGeneratesExpectedCards()
//     {
//         // Arrange
//         var expectedCards = new List<Card>
//         {
//             new Card("2", "Hearts"),
//             new Card("3", "Hearts"),
//             new Card("4", "Hearts"),
//             new Card("J", "Hearts"),
//             new Card("Q", "Hearts"),
//             new Card("K", "Hearts"),
//             new Card("2", "Diamonds"),
//             new Card("3", "Diamonds"),
//             new Card("4", "Diamonds"),
//             new Card("J", "Diamonds"),
//             new Card("Q", "Diamonds"),
//             new Card("K", "Diamonds")
//         };

//         // Act
//         var cards = _cardGenerator.Generate(_deckDescriptor);

//         // Assert
//         CollectionAssert.AreEqual(expectedCards, cards);
//     }
//     [Test]
//     public void GenerateCardsDeckDescriptorIsNullReturnsEmptyList()
//     {
//         // Arrange

//         // Act
//         var cards = _cardGenerator.Generate(null);

//         // Assert
//         CollectionAssert.IsEmpty(cards);
//     }
// }

[TestFixture]
public class CardGeneratorTests
{
    private static ILogger _logger = new ConsoleLogger();
    private static CardGenerator _cardGenerator = new CardGenerator(_logger);

    private static DeckDescriptor _deckDescriptor1 = new DeckDescriptor(new[] { 2, 3, 4 }, new[] { "J", "Q", "K" }, new[] { "Hearts", "Diamonds" });
    private static DeckDescriptor _deckDescriptor2 = new DeckDescriptor(new[] { 1, 2 }, new[] { "A", "B" }, new[] { "Clubs", "Spades" });
    private static readonly object[] TestCases =
    {
        new object[] { new DeckDescriptor(new[] { 2, 3, 4 }, new[] { "J", "Q", "K" }, new[] { "Hearts", "Diamonds" }), 12 },
        new object[] { new DeckDescriptor(new[] { 1, 2 }, new[] { "A", "B" }, new[] { "Clubs", "Spades" }), 8 }
    };
    private static readonly object[] TestCases2 =
    {
        new object[] { new DeckDescriptor(new[] { 2, 3, 4 }, new[] { "J", "Q", "K" }, new[] { "Hearts", "Diamonds" })},
        new object[] { new DeckDescriptor(new[] { 1, 2 }, new[] { "A", "B" }, new[] { "Clubs", "Spades" })}
    };



    [TestCaseSource(nameof(TestCases))]
    public void GenerateCardsReturnsExpectedNumberOfCards(DeckDescriptor deckDescriptor, int expectedCardCount)
    {
        // Arrange

        // Act
        var cards = _cardGenerator.Generate(deckDescriptor);

        // Assert
        Assert.That(cards.Count, Is.EqualTo(expectedCardCount));
    }
    [TestCaseSource(nameof(TestCases2))]
    public void GenerateCardsGeneratesExpectedCards(DeckDescriptor deckDescriptor)
    {
        // Arrange
        var expectedCards = new List<Card>
        {
            new Card("2", "Hearts"),
            new Card("3", "Hearts"),
            new Card("4", "Hearts"),
            new Card("J", "Hearts"),
            new Card("Q", "Hearts"),
            new Card("K", "Hearts"),
            new Card("2", "Diamonds"),
            new Card("3", "Diamonds"),
            new Card("4", "Diamonds"),
            new Card("J", "Diamonds"),
            new Card("Q", "Diamonds"),
            new Card("K", "Diamonds")
        };

        // Act
        var cards = _cardGenerator.Generate(deckDescriptor);

        // Assert
        CollectionAssert.AreEqual(expectedCards, cards);
    }

}

[TestFixture]
public class DeckBuilderTest
{
    private Mock<ICardGenerator> _cardGeneratorMock;
    private DeckDescriptor _deckDescriptor;

    [SetUp]
    public void SetUp() // This method runs before every test
    {
        _cardGeneratorMock = new Mock<ICardGenerator>(); // We create the mock object and store it in a class variable
        _deckDescriptor = new DeckDescriptor(Array.Empty<int>(), Array.Empty<string>(), Array.Empty<string>()); //DeckDescriptor contents doesn't matter - we just need to create a new DeckDescriptor object
    }

    [Test]
    public void CreateDeck_ReturnsNewDeckWithGeneratedCards()
    {
        // Arrange
        var cards = new List<Card> { new("Ace", "Spades"), new Card("Ace", "Hearts") };
        _cardGeneratorMock.Setup(x => x.Generate(_deckDescriptor)).Returns(cards); // Here, we set up that the mock should return the cards that we specified
        var deckBuilder = new DeckBuilder(_cardGeneratorMock.Object, _deckDescriptor);

        // Act
        var deck = deckBuilder.CreateDeck();

        // Assert
        var drawn = deck.DrawOne();
        while (drawn != null)
        {
            Assert.That(cards, Does.Contain(drawn));
            drawn = deck.DrawOne();
        }
    }
}
