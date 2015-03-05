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
    public class VideoScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewVideoIsPersisted()
        {

            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    Publisher = "Bill Nye",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);

                bool exists = bc.DataContext.Videos.Any(c => c.Id == video.Id);

                Assert.IsTrue(exists);
            }
        }
        
        [TestMethod]
        public void AddNewVideoIsPersistedWhenOptionalFieldsAreNull()
        {

            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "Video",
                    VideoFormat = "DVD",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);

                bool exists = bc.DataContext.Videos.Any(c => c.Id == video.Id);

                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void GetAllVideosReturnsListOfAllDatabaseEntries()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);

                var video2 = new Video
                {
                    Title = "Video2",
                    EAN = "1234567891123",
                    Publisher = "Bill Wye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 2,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video2);

                var VideoList = bc.GetAllVideos();

                bool exists = VideoList.Any(c => c.Id == video.Id);
                bool exists2 = VideoList.Any(c => c.Id == video2.Id);

                Assert.IsTrue(exists);
                Assert.IsTrue(exists2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewVideoThrowsExceptionIfIdAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                    {
                        Title = "Video",
                        EAN = "1234567890123",
                        Publisher = "Bill Nye",
                        AudienceRating = "Unrated",
                        VideoFormat = "DVD",
                        VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                        Items_Video = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "Video",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }
                    };

                var video2 = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Vye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                            new Item
                            {
                                Id = 1,
                                ItemType = "Video",
                                ListedPrice = 3.00,
                                ListedDate = DateTime.Parse("12/25/2014"),
                                Subject = "Math K-2",
                                Status = "Shelved",
                                Condition = "Good"
                            }
                        }

                };
            
                bc.AddNewVideo(video);
                bc.AddNewVideo(video2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewVideoThrowsExceptionIfMoreThanOneIdIsAdded()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        },
                        new Item
                        {
                            Id = 2,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);
            }
        }

        [TestMethod]
        public void GetItemByIsbnReturnsCorrectDatabaseEntry()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);

                Video TestVideo = bc.GetVideoByTitle(video.Title);

                bool isThere = TestVideo.Id == video.Id;

                Assert.IsTrue(isThere);

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewVideoThrowsExceptionIfTitleIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = null,
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewVideoThrowsExceptionIfTitleIsEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                bc.AddNewVideo(video);
            }
        }

        [TestMethod]
        public void AddNewVideoUpdatesVideoTableIfEanAlreadyExists()
        {
            using (var bc = new BusinessContext())
            {
                var video = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            //Id = 1,
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };
                bc.AddNewVideo(video);

                var video2 = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    VideoImage = "http://ecx.images-amazon.com/images/I/51VA8Jvun2L._AA160_.jpg",
                    Items_Video = new List<Item>
                    {
                        new Item
                        {
                            //Id = 2,
                            ItemType = "Video",
                            ListedPrice = 5.00,
                            ListedDate = DateTime.Parse("12/23/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good"
                        }
                    }
                };

                
                bc.AddNewVideo(video2);

                List<Video> results = bc.GetAllVideos();

                Assert.IsFalse(results.Count(b => b.EAN == video.EAN) != 1);
                Assert.IsTrue(results.FirstOrDefault(b => b.Id == video.Id).Items_Video.Count == 2);
            }
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        public void AddNewVideoThrowsExceptionIfVideoIsNotNull()
        {
            using (var bc = new BusinessContext())
            {
                Video testVideo = new Video();

                var video = new Video
                {
                    Title = "Video",
                    EAN = "1234567890123",
                    Publisher = "Bill Nye",
                    AudienceRating = "Unrated",
                    VideoFormat = "DVD",
                    Items_Video = new List<Item>{
                        new Item
                        {
                            ItemType = "Video",
                            ListedPrice = 3.00,
                            ListedDate = DateTime.Parse("12/25/2014"),
                            Subject = "Math K-2",
                            Status = "Shelved",
                            Condition = "Good",
                            Video = testVideo
                        }
                    }
                };
                bc.AddNewVideo(video);
            }
        }

    }
}
