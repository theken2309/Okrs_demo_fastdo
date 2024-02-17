using MongoDB.Bson.Serialization.Attributes;

namespace OKRs_Fastdo.Models
{
    public class Okr_suggestModel
    {
        [BsonId]
        public string id { get; set; }
        // ten gop y 
        public string name { get; set; }
        // chu ky
        public string cycle { get; set; }
        // kieu cua gop y 
        public string type { get; set; }
        // time tao
        public long date { get; set; }
        // cua user nao 
        public string user { get; set; }
    }
}
