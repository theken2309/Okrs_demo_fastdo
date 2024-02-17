using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbOkrCheckinTime
    {
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<OkrCheckITimeModel> _OKRCheckinTable = _data.GetCollection<OkrCheckITimeModel>("okr_checkinTime");
        public static async Task Create(OkrCheckITimeModel model )
        {
            model.id = MongoDb.RandomId();
            Console.WriteLine("click them okr checkin");

            _OKRCheckinTable.InsertOne(model);
        }
        public static Task<List<OkrCheckITimeModel>> GetAll()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _OKRCheckinTable.FindAsync(x => true).Result.ToListAsync();
        }
        public static async Task Delete(string id)
        {
            var filter = Builders<OkrCheckITimeModel>.Filter.Eq("_id", id); // Giả sử "_id" là tên của trường ObjectId trong MongoDB

            var ketQua = await _OKRCheckinTable.DeleteOneAsync(filter);

            if (ketQua.DeletedCount > 0)
            {
                Console.WriteLine($"Đã xóa {ketQua.DeletedCount} tài liệu có id {id}");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy tài liệu nào có id {id}");
            }
        }
        public static async Task<OkrCheckITimeModel> GetTheoDateCreate(OkrCheckITimeModel model) // truyen okr va date checkin vao de lay 
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckITimeModel>("okr_checkinTime");

            // Truy vấn MongoDB dựa trên giá trị của trường "okr" và "checkin" trong đối tượng "model"
            var filter = Builders<OkrCheckITimeModel>.Filter.And(
                Builders<OkrCheckITimeModel>.Filter.Eq(x => x.okr, model.okr),
                Builders<OkrCheckITimeModel>.Filter.Eq(x => x.date_checkin, model.date_checkin)
            //Builders<OkrCheckInModel>.Filter.Eq(x => x.date_create, model.date_create)

            );

            var result = await collection.FindAsync(filter).Result.FirstOrDefaultAsync();

            return result;
        }
        public static string Update(OkrCheckITimeModel okr)
        {
            try
            {
                //lọc okr theo id , filter sẽ lấy okr cần update
                var filter = Builders<OkrCheckITimeModel>.Filter.Eq(x => x.id, okr.id);
                // cần cập nhật gì thì set nó
                var update = Builders<OkrCheckITimeModel>.Update
                    .Set(x => x.status, 2);
                // cập nhật 
                var result = _OKRCheckinTable.UpdateOne(filter, update);

                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine("Cập nhật thành công");
                    return "Updated successfully";
                }
                else
                {
                    Console.WriteLine("Không tìm thấy bản ghi để cập nhật");
                    return "Record not found for update";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật: " + ex.Message);
                return "Error updating record: " + ex.Message;
            }
        }
    }
}
