using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using RocketPos.Data.DataLayer.Entities;
using RocketPos.Data.TransactionalLayer;

namespace RocketPos.Tests.DataTests.Functional_Tests
{
    [TestClass]
    public class GameScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewGameIsPersisted()
        {

            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            //Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);

                bool exists = bc.DataContext.Games.Any(c => c.Id == game.Id);

                Assert.IsTrue(exists);
            }
        }
        
        [TestMethod]
        public void AddNewGameIsPersistedWhenOptionalFieldsAreNull()
        {

            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "Game",
                    Manufacturer = null,
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);

                bool exists = bc.DataContext.Games.Any(c => c.Id == game.Id);

                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void GetAllGamesReturnsListOfAllDatabaseEntries()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);

                var game2 = new Game
                {
                    Title = "Game2",
                    EAN = "1234567891123",
                    Manufacturer = "Bill Wye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 2,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game2);

                var GameList = bc.GetAllGames();

                bool exists = GameList.Any(c => c.Id == game.Id);
                bool exists2 = GameList.Any(c => c.Id == game2.Id);

                Assert.IsTrue(exists);
                Assert.IsTrue(exists2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewGameThrowsExceptionIfIdAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                    {
                        Title = "Game",
                        EAN = "1234567890123",
                        Manufacturer = "Bill Nye",
                        GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                        Items_Games = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "Game",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }
                    };

                var game2 = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Vye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "Game",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }

                };
            
                bc.AddNewGame(game);
                bc.AddNewGame(game2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewGameThrowsExceptionIfMoreThanOneIdIsAdded()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        },
                        new Item
                        {
                            Id = 2,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);
            }
        }

        [TestMethod]
        public void GetItemByIsbnReturnsCorrectDatabaseEntry()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);

                Game TestGame = bc.GetGameByTitle(game.Title);

                bool isThere = TestGame.Id == game.Id;

                Assert.IsTrue(isThere);

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewGameThrowsExceptionIfTitleIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = null,
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewGameThrowsExceptionIfTitleIsEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewGame(game);
            }
        }

        [TestMethod]
        public void AddNewGameUpdatesGameTableIfEanAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var game = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            //Id = 1,
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };
                bc.AddNewGame(game);

                var game2 = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    GameImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Games = new List<Item>
                    {
                        new Item
                        {
                            //Id = 2,
                            ItemType = "Game",
                            ListedPrice = 5.00,
                            ListedDate = DateTime.Parse("12/23/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                
                bc.AddNewGame(game2);

                List<Game> results = bc.GetAllGames();

                Assert.IsFalse(results.Count(b => b.EAN == game.EAN) != 1);
                Assert.IsTrue(results.FirstOrDefault(b => b.Id == game.Id).Items_Games.Count == 2);
            }
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void AddNewGameThrowsExceptionIfGameIsNotNull()
        {
            using (var bc = new BusinessContext())
            {
                Game testGame = new Game();

                var game = new Game
                {
                    Title = "Game",
                    EAN = "1234567890123",
                    Manufacturer = "Bill Nye",
                    Items_Games = new List<Item>{
                        new Item
                        {
                            ItemType = "Game",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            Game = testGame
                        }
                    }
                };
                bc.AddNewGame(game);
            }
        }

    }
}
