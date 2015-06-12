using System.Collections.Generic;


namespace RocketPos.Common.Helpers
{


    public static class EnumsAndLists
    {
        public static readonly List<string> ItemTypes = new List<string>()
        {
            "Book",
            "Game",
            "Teaching Aide",
            "Video",
            "Other"
        };

        public static readonly List<string> Statuses = new List<string>()
        {
            "Not yet arrived in store",
            "Arrived but not shelved",
            "Shelved",
            "Lost",
            "Sold",
            "Paid",
            "Pending"
        };

        public static readonly List<string> Conditions = new List<string>()
        {
            "New",
            "Like New",
            "Good",
            "Fair",
            "Beat Up"
        };

        public static readonly List<string> VideoFormats = new List<string>()
        {
            "DVD",
            "VHS",
            "Blu-Ray"
        };

        public static readonly List<string> Ratings = new List<string>()
        {
            "G (General Audience)",
            "PG (Parental Guidance Suggested)",
            "PG-13 (Parental Guidance Suggested)",
            "R (Restricted Audience)",
            "NR (Not Rated)",
            "Unrated"
        };

        public static readonly List<string> Bindings = new List<string>()
        {
            "Hardcover",
            "Softcover",
            "Audio CD",
            "Spiral-bound",
            "Board book",
            "Map"
        };

        public static readonly List<string> Subjects = new List<string>()
        {
            "Math K-5",
            "Math 6-8",
            "Math 9-12",
            "Math Other",
            "Reading Easy-Readers",
            "Reading 1-3",
            "Reading 4-6",
            "Reading 7-12",
            "Reading Biography",
            "Reading Other",
            "Curriculum K-2",
            "Curriculum 3-5",
            "Curriculum 6-8",
            "Curriculum 9-12",
            "History K-6",
            "History 7-12",
            "Science K-6",
            "Science 7-12",
            "English K-6",
            "English 7-12",
            "Music",
            "Foreign Language",
            "Art",
            "Games",
            "Videos",
            "Preschool",
            "Teaching Aides",
            "Parent/Teacher",
            "Other"

        };

        public static readonly List<string> States = new List<string>()
        {
            "AL",
            "AK",
            "AZ",
            "AR",
            "CA",
            "CO",
            "CT",
            "DE",
            "FL",
            "GA",
            "HI",
            "ID",
            "IL",
            "IN",
            "IA",
            "KS",
            "KY",
            "LA",
            "ME",
            "MD",
            "MA",
            "MI",
            "MN",
            "MS",
            "MO",
            "MT",
            "NE",
            "NV",
            "NH",
            "NJ",
            "NM",
            "NY",
            "NC",
            "ND",
            "OH",
            "OK",
            "OR",
            "PA",
            "RI",
            "SC",
            "SD",
            "TN",
            "TX",
            "UT",
            "VT",
            "VA",
            "WA",
            "WV",
            "WI",
            "WY"

        };

        public static readonly List<string> MemberTerms = new List<string>()
        {
            "Full-Year 2015 (July1-Jun30)",
            "Fall Semester 2015 (July1-Dec31)",
            "Spring Semester 2015 (Jan1-Jun30)"
        };

        public static readonly Dictionary<string, int> MemberTypes = new Dictionary<string, int>()
        {
            {"Standard",1},
            {"Volunteer",2},
            {"Teacher - 1 Class",3},
            {"Teacher - 2 Classes",4},
            {"Teacher - 3 Classes",5},
            {"Teacher - 4 Classes",6}
        };
        
        public static readonly Dictionary<int, string> MemberTypesInts = new Dictionary<int, string>()
        {
            {1,"Standard"},
            {2,"Volunteer"},
            {3,"Teacher - 1 Class"},
            {4,"Teacher - 2 Classes"},
            {5,"Teacher - 3 Classes"},
            {6,"Teacher - 4 Classes"}
        };
    }
}
