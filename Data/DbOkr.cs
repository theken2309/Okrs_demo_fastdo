using Microsoft.JSInterop;
using MongoDB.Bson;
using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbOkr
    {
        //kết nối bới collection okr trong DB OKR_Ken
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<okrModel> _OKRTable = _data.GetCollection<okrModel>("okr");

        //lay danh sach cua OKR
        public static Task<List<okrModel>> GetAll()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _OKRTable.FindAsync(x => true).Result.ToListAsync();
        }

        //tạo okr mới
        public static string Create(okrModel okr)
        {
             try
            {
                
                okr.id = MongoDb.RandomId();
                _OKRTable.InsertOne(okr);
                Console.WriteLine("tao dc roi");
                return "Created successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An lz luon");

                return $"Error creating user: {ex.Message}";
            }
        }
        //cập nhật okr
        public static string Update(okrModel okr)
        {
            try
            {
                //lọc okr theo id , filter sẽ lấy okr cần update
                var filter = Builders<okrModel>.Filter.Eq(x => x.id, okr.id);
                // cần cập nhật gì thì set nó
                var update = Builders<okrModel>.Update
                    .Set(x => x.name, okr.name) 
                    .Set(x=>x.type, okr.type)
                    .Set(x => x.parent, okr.parent)
                    .Set(x => x.review_send_date, okr.review_send_date)
                    .Set(x => x.review_manager_date, okr.review_manager_date)
                    .Set(x => x.key_results, okr.key_results);
                // cập nhật 
                var result = _OKRTable.UpdateOne(filter, update);

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
        //xoá okr
        public static string Delete(okrModel okr)
        {
            try
            {
                //lọc okr theo id, filter sẽ lấy okr cần xóa
                var filter = Builders<okrModel>.Filter.Eq(x => x.id, okr.id);

                // xóa bản ghi
                var result = _OKRTable.DeleteOne(filter);

                if (result.DeletedCount > 0)
                {

                    Console.WriteLine("Xóa thành công");
                    return "Deleted successfully";
                }
                else
                {
                    Console.WriteLine("Không tìm thấy bản ghi để xóa");
                    return "Record not found for delete";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa: " + ex.Message);
                return "Error deleting record: " + ex.Message;
            }
        }
        // lấy okr dựa trên id
        public static async Task<okrModel> Get(okrModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken"); // Thay thế "your_database_name" bằng tên của cơ sở dữ liệu MongoDB

            var collection = _db.GetCollection<okrModel>("okr"); // Thay thế "your_collection_name" bằng tên của collection

            // Truy vấn MongoDB dựa trên giá trị của thuộc tính "id" trong đối tượng "model"
            var result = await collection.FindAsync(x => x.id == model.id).Result.FirstOrDefaultAsync();

            return result;
        }
        // set trạng thái checkin của okr
        public static async Task Set_statusCheckin(okrModel okrset, int newstatus)
        {
            // Kết nối đến cơ sở dữ liệu MongoDB và lấy collection
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<okrModel>("okr");

            // Tìm và thay thế dựa trên giá trị của thuộc tính "id"
            var filter = Builders<okrModel>.Filter.Eq(x => x.id, okrset.id);

            // Tạo một cập nhật để chỉ cập nhật thuộc tính "cycle"
            var update = Builders<okrModel>.Update
                .Set(x => x.status_checkin, newstatus)
                .Set(x => x.confident, okrset.confident)
                .Set(x => x.date_checkin, okrset.date_checkin)
                .Set(x => x.done, okrset.done)
                .Set(x => x.key_results, okrset.key_results);

            try
            {
                // Thực hiện cập nhật
                var result = await collection.UpdateOneAsync(filter, update);

                // Kiểm tra xem có bản ghi nào được cập nhật không
                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine($"Đã cập nhật thành công {result.ModifiedCount} bản ghi");
                }
                else
                {
                    Console.WriteLine("Không có bản ghi nào được cập nhật");
                }
            }
            catch (Exception ex)
            {
                // Ghi log khi có lỗi
                Console.WriteLine($"Lỗi khi cập nhật: {ex.Message}");
                // Hoặc sử dụng một thư viện ghi log như Serilog, NLog, ...
            }
        }

        //set xem okr đã hoàn thành hay chưa 
        public static async Task SetDone(okrModel okrset)
        {
            // Kết nối đến cơ sở dữ liệu MongoDB và lấy collection
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<okrModel>("okr");

            // Tìm và thay thế dựa trên giá trị của thuộc tính "id"
            var filter = Builders<okrModel>.Filter.Eq(x => x.id, okrset.id);

            // Tạo một cập nhật để chỉ cập nhật thuộc tính "cycle"
            var update = Builders<okrModel>.Update
                .Set(x => x.key_results, okrset.key_results)
                .Set(x => x.confident, okrset.confident)
                .Set(x => x.delete, true)
                .Set(x => x.done, okrset.done);

            try
            {
                // Thực hiện cập nhật
                var result = await collection.UpdateOneAsync(filter, update);

                // Kiểm tra xem có bản ghi nào được cập nhật không
                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine($"Đã cập nhật thành công {result.ModifiedCount} bản ghi");
                }
                else
                {
                    Console.WriteLine("Không có bản ghi nào được cập nhật");
                }
            }
            catch (Exception ex)
            {
                // Ghi log khi có lỗi
                Console.WriteLine($"Lỗi khi cập nhật: {ex.Message}");
                // Hoặc sử dụng một thư viện ghi log như Serilog, NLog, ...
            }
        }
        // lấy list những okr mà của use tạo ra dựa trên id user
        public static async Task<List<okrModel>> GetByUserID(string userID)
        {
            return await _OKRTable.FindAsync(x => x.user_create == userID).Result.ToListAsync();
        }

    }

}
