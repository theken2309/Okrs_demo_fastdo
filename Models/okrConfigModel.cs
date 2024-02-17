using MongoDB.Bson.Serialization.Attributes;

namespace OKRs_Fastdo.Models
{
    public class okrConfigModel
    {
        [BsonId]
        public string id { get; set; }


        public List<Cycle> cycles { get; set; } = new();

        public List<Suggest> suggests { get; set; } = new();

        public List<Evaluate> evaluates { get; set; } = new();


        //chu kì
        public class Cycle
        {
            public string id { get; set; }
            public string name { get; set; }
            public long start { get; set; }
            public long end { get; set; }
            public bool done { get; set; }
        }

        public class Suggest
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Evaluate
        {
            public string id { get; set; }
            public string name { get; set; }
            public int type { get; set; } // 1. Phản hồi | 2. Ghi nhận
            public bool special { get; set; } // Ghi nhận dặt biệt
        }
    }
}
