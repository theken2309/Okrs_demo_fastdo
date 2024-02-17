using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbOkr_suggest
    {
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<Okr_suggestModel> _OkrSuggestTable = _data.GetCollection<Okr_suggestModel>("okr_suggest");
        //lay danh sach cua OKR
        public static Task<List<Okr_suggestModel>> GetAll()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _OkrSuggestTable.FindAsync(x => true).Result.ToListAsync();
        }
        // tạo ra góp ý mới
        public static void Create(Okr_suggestModel model)
        {
            model.id = MongoDb.RandomId();
            _OkrSuggestTable.InsertOne(model);
        }
    }
}
