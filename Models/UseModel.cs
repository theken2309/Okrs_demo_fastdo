using MongoDB.Bson.Serialization.Attributes;

namespace OKRs_Fastdo.Models
{
    public class UseModel
    {
        [BsonId]
        public string id { get; set; }
        public string nameUse { get; set; }
        // quyền của Use ,  1 Nhân Viên  // 2 Quản Lý //3 admin
        public int permisson { get; set; }
        public string password { get; set; }

    }
}
