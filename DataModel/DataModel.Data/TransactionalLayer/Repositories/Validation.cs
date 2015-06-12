using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    static class Validation
    {
        public static void StringRequire(string value)
        {
            if (value == null)
                throw new NullReferenceException("Item string member must not be null");
            if (value.Trim().Length == 0)
                throw new ArgumentException("Item string member must not be empty or null");
        }

        public static void DateRequire(DateTime value)
        {
            if (value == null)
                throw new NullReferenceException("DateTime must not be null");
            if (value.ToString().Trim().Length == 0)
                throw new ArgumentException("DateTime member must not be empty or null");
        }


        public static void PriceRequire(double value)
        {
            if (value <= 0)
                throw new NullReferenceException("price member must be greater than zero");

            if (Double.IsNaN(value))
                throw new ArgumentException("Price member must be a number");

        }

        //public static void BoolRequire(bool value)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException("Book", "Book boolean member must not be null");
        //}

        internal static void NameLength(string value)
        {
            if (value != null)
                if (value.Length > 20)
                    value.Remove(20);
        }

        internal static void EmailLength(string value)
        {
            if (value != null)
                if (value.Length > 40)
                    value.Remove(40);
        }

        internal static void AddressLength(string value)
        {
            if (value != null)
                if (value.Length > 40)
                    value.Remove(40);
        }

        internal static void CityLength(string value)
        {
            if (value != null)
                if (value.Length > 30)
                    value.Remove(30);
        }

        internal static void StateLength(string value)
        {
            if (value != null)
                if (value.Length > 2)
                    value.Remove(2);
        }

        internal static void ZipLength(string value)
        {
            if (value != null)
                if (value.Length > 10)
                    value.Remove(10);
        }

        internal static void PhoneLength(string value)
        {
            if (value != null)
                if (value.Length > 10)
                    value.Remove(10);
        }

        internal static void TitleLength(string value)
        {
            if (value != null)
                if (value.Length > 150)
                    value.Remove(150);
        }

        internal static void AuthorLength(string value)
        {
            if (value != null)
                if (value.Length > 100)
                    value.Remove(100);
        }

        internal static void ImageLength(string value)
        {
            if (value != null)
                if (value.Length > 300)
                    throw new ArgumentException("Image URL is longer than 200 characters");
        }

        internal static void IsbnLength(string value)
        {
            if (value != null)
                if (value.Length > 13)
                    throw new ArgumentException("ISBN in Book object must be 13 characters or less");
        }

        internal static void EanLength(string value)
        {
            if (value != null)
                if (value.Length > 13)
                    throw new ArgumentException("EAN in object must be 13 characters or less");
        }


        internal static void VideoFormatLength(string value)
        {
            if (value != null)
                if (value.Length > 7)
                    throw new ArgumentException("VideoFormat in video must be 7 characters or less");
        }

        internal static void AudienceRatingLength(string value)
        {
            if (value != null)
                if (value.Length > 40)
                    throw new ArgumentException("AudienceRating in video must be 20 characters or less");
        }


        internal static void BooksDuplicate(List<Book> books, Book book)
        {

            //Check for duplicate ISBN
            if (books.Any(b => b.ISBN == book.ISBN))
                throw new ArgumentException("ISBN in Book object already exists");
        }

        #region ItemDuplicate Methods
        internal static void ItemDuplicate(Book book)
        {
            if (book.Items_TItems != null)
                if (book.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to book at a time");
                else
                {
                    if (book.Items_TItems.Count != book.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Book already contains specified Id, cannot add duplicate Id to Book");
                }
        }

        internal static void ItemDuplicate(Book book, Book oldBook)
        {
            if (book.Items_TItems != null)
            {
                if (book.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to book at a time");
                else
                {
                    if (book.Items_TItems.Count != book.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Book already contains specified Id, cannot add duplicate Id to Book");
                    if (oldBook.Items_TItems.Any(b => b.Id == book.Items_TItems.ElementAt(0).Id))
                        throw new ArgumentException("A Book exists that already contains specified Id, cannot add duplicate Id to Book");
                }
            }
        }

        internal static void ItemDuplicate(Game game)
        {
            if (game.Items_TItems != null)
                if (game.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to game at a time");
                else
                {
                    if (game.Items_TItems.Count != game.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to Game");
                }
        }

        internal static void ItemDuplicate(Game game, Game oldGame)
        {
            if (game.Items_TItems != null)
            {
                if (game.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to game at a time");
                else
                {
                    if (game.Items_TItems.Count != game.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to Game");
                    if (oldGame.Items_TItems.Any(b =>
                    {
                        var firstOrDefault = game.Items_TItems.FirstOrDefault();
                        return firstOrDefault != null && b.Id == firstOrDefault.Id;
                    }))
                        throw new ArgumentException("A Game exists that already contains specified Id, cannot add duplicate Id to Game");
                }
            }
        }

        internal static void ItemDuplicate(TeachingAide teachingAide)
        {
            if (teachingAide.Items_TItems != null)
                if (teachingAide.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to teachingAide at a time");
                else
                {
                    if (teachingAide.Items_TItems.Count != teachingAide.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to TeachingAide");
                }
        }

        internal static void ItemDuplicate(TeachingAide teachingAide, TeachingAide oldTeachingAide)
        {
            if (teachingAide.Items_TItems != null)
            {
                if (teachingAide.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to teachingAide at a time");
                if (teachingAide.Items_TItems.Count != teachingAide.Items_TItems.Distinct().Count())
                    throw new ArgumentException("Game already contains specified Id, cannot add duplicate Id to TeachingAide");
                if (oldTeachingAide.Items_TItems.Any(b =>
                {
                    var firstOrDefault = teachingAide.Items_TItems.FirstOrDefault();
                    return firstOrDefault != null && b.Id == firstOrDefault.Id;
                }))
                    throw new ArgumentException("A TeachingAide exists that already contains specified Id, cannot add duplicate Id to TeachingAide");
            }
        }

        internal static void ItemDuplicate(Other other)
        {
            if (other.Items_TItems != null)
                if (other.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to other at a time");
                else
                {
                    if (other.Items_TItems.Count != other.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Other already contains specified Id, cannot add duplicate Id to Other");
                }
        }

        internal static void ItemDuplicate(Other other, Other oldOther)
        {
            if (other.Items_TItems != null)
            {
                if (other.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to other at a time");
                else
                {
                    if (other.Items_TItems.Count != other.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Other already contains specified Id, cannot add duplicate Id to Other");
                    if (oldOther.Items_TItems.Any(b =>
                    {
                        var firstOrDefault = other.Items_TItems.FirstOrDefault();
                        return firstOrDefault != null && b.Id == firstOrDefault.Id;
                    }))
                        throw new ArgumentException("A Other exists that already contains specified Id, cannot add duplicate Id to Other");
                }
            }
        }

        internal static void ItemDuplicate(Video video)
        {
            if (video.Items_TItems != null)
                if (video.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to video at a time");
                else
                {
                    if (video.Items_TItems.Count != video.Items_TItems.Distinct().Count())
                        throw new ArgumentException("Video already contains specified Id, cannot add duplicate Id to Video");
                }
        }

        internal static void ItemDuplicate(Video video, Video oldVideo)
        {
            if (video.Items_TItems != null)
            {
                if (video.Items_TItems.Count > 1)
                    throw new ArgumentException("Cannot add more than one Item to video at a time");
                if (video.Items_TItems.Count != video.Items_TItems.Distinct().Count())
                    throw new ArgumentException("Video already contains specified Id, cannot add duplicate Id to Video");
                if (oldVideo.Items_TItems.Any(b =>
                {
                    var firstOrDefault = video.Items_TItems.FirstOrDefault();
                    return firstOrDefault != null && b.Id == firstOrDefault.Id;
                }))
                    throw new ArgumentException("A Video exists that already contains specified Id, cannot add duplicate Id to Video");
            }
        }
        # endregion

        internal static void ConsignorPersonDuplicate(Consignor consignor, Consignor oldConsignor)
        {
            if (consignor.Consignor_Person != null)
            {
                if (oldConsignor.Consignor_Person != null)
                    throw new ArgumentException("Consignor already contains Id, cannot change Id of Consignor");
                if (oldConsignor.Consignor_Person != null && oldConsignor.Consignor_Person.Id == consignor.Consignor_Person.Id)
                    throw new ArgumentException("A Consignor exists that already contains specified Id, cannot add duplicate Id to Consignor");

            }
        }

        internal static void MemberPersonDuplicate(Member member, Member oldMember)
        {
            if (member.Member_Person != null)
            {
                if (oldMember.Member_Person != null)
                    throw new ArgumentException("Member already contains Id, cannot change Id of Member");
                if (oldMember.Member_Person != null && oldMember.Member_Person.Id == member.Member_Person.Id)
                    throw new ArgumentException("A Member exists that already contains specified Id, cannot add duplicate Id to Member");

            }
        }

        internal static void ConsignorPmtDuplicate(ConsignorPmt consignorPmt, ConsignorPmt oldConsignorPmt)
        {
            if (consignorPmt.Items_ConsignorPmt.Count > 1)
                throw new ArgumentException("Cannot add more than one Item to ConsignorPmt at a time");
            else
            {
                if (consignorPmt.Items_ConsignorPmt.Count != consignorPmt.Items_ConsignorPmt.Distinct().Count())
                    throw new ArgumentException("ConsignorPmt already contains specified Id, cannot add duplicate Id to ConsignorPmt");
                if (oldConsignorPmt.Items_ConsignorPmt.Any(b =>
                {
                    var firstOrDefault = consignorPmt.Items_ConsignorPmt.FirstOrDefault();
                    return firstOrDefault != null && b.Id == firstOrDefault.Id;
                }))
                    throw new ArgumentException("A ConsignorPmt exists that already contains specified Id, cannot add duplicate Id to ConsignorPmt");
            }

            if (consignorPmt.Consignor_ConsignorPmt != null)
            {
                if (oldConsignorPmt.Consignor_ConsignorPmt != null)
                    throw new ArgumentException("ConsignorPmt already contains Consignor, cannot change Id of Consignor");
                if (oldConsignorPmt.Consignor_ConsignorPmt != null && oldConsignorPmt.Consignor_ConsignorPmt.Id == consignorPmt.Consignor_ConsignorPmt.Id)
                    throw new ArgumentException("A ConsignorPmt exists that already contains specified Id, cannot add duplicate Id to Consignor");
            }

            if (consignorPmt.DebitTransaction_ConsignorPmt != null)
            {
                if (oldConsignorPmt.DebitTransaction_ConsignorPmt != null)
                    throw new ArgumentException("ConsignorPmt already contains DebitTransaction");
                if (oldConsignorPmt.DebitTransaction_ConsignorPmt != null && oldConsignorPmt.DebitTransaction_ConsignorPmt.Id == consignorPmt.DebitTransaction_ConsignorPmt.Id)
                    throw new ArgumentException("A ConsignorPmt exists that already contains specified Id, cannot add duplicate Id to ConsignorPmt");
            }
        }
    }
}
