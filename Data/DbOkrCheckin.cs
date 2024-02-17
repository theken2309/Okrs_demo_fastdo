using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbOkrCheckin
    {  //kết nối bới collection okr trong DB OKR_Ken
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<OkrCheckInModel> _OKRCheckinTable = _data.GetCollection<OkrCheckInModel>("okrCheckin");

        //lay danh sach cua OKR
        public static Task<List<OkrCheckInModel>> GetAll()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _OKRCheckinTable.FindAsync(x => true).Result.ToListAsync();
        }
        // tạo okrCHeckin
        public static async Task Create(OkrCheckInModel model)
        {
            model.id = MongoDb.RandomId();
            Console.WriteLine("click them okr checkin");

            _OKRCheckinTable.InsertOne(model);
        }
        //update okr checkin 
        public static void Update(OkrCheckInModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            // Xây dựng bộ lọc dựa trên giá trị của trường "id" trong đối tượng "model"
            var filter = Builders<OkrCheckInModel>.Filter.Eq(x => x.id, model.id);

                    // Xây dựng các cập nhật (set) dựa trên các trường cần cập nhật
                    var update = Builders<OkrCheckInModel>.Update
                        .Set(x => x.draft, model.draft) // Thay thế Property1 với giá trị mới từ model
                        .Set(x => x.user_checkin, model.user_checkin) // Thay thế Property1 với giá trị mới từ model
                        .Set(x => x.date_checkin, model.date_checkin) // Thay thế Property1 với giá trị mới từ model
                        .Set(x => x.progress_prev, model.progress_prev) // Thay thế Property1 với giá trị mới từ model
                        .Set(x => x.key_results, model.key_results) // Thay thế Property1 với giá trị mới từ model
                        .Set(x => x.review_send_date, model.review_send_date) // Thay thế Property1 với giá trị mới từ model
                        .Set(x => x.confident, model.confident);// Thay thế Property2 với giá trị mới từ model
                // Thêm các trường cần cập nhật tương ứng ở đây

            // Thực hiện cập nhật
                         var result = collection.UpdateOne(filter, update);

            Console.WriteLine($"Số lượng tài liệu được cập nhật: {result.ModifiedCount}");
        }
        // get okr checkin dựa trên id
        public static async Task<OkrCheckInModel> Get(OkrCheckInModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken"); // Thay thế "your_database_name" bằng tên của cơ sở dữ liệu MongoDB

            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin"); // Thay thế "your_collection_name" bằng tên của collection

            // Truy vấn MongoDB dựa trên giá trị của thuộc tính "id" trong đối tượng "model"
            var result = await collection.FindAsync(x => x.id == model.id).Result.FirstOrDefaultAsync();

            return result;
        }
        //get theo okr
        public static async Task<OkrCheckInModel> GetTheoDateCreate(OkrCheckInModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            var filter = Builders<OkrCheckInModel>.Filter.And(
                Builders<OkrCheckInModel>.Filter.Eq(x => x.okr, model.okr)
            );
            var result = await collection.FindAsync(filter).Result.ToListAsync();
            return result.LastOrDefault();
        }

        //get theo okr và time tạo
        public static async Task<OkrCheckInModel> GetTheoTime(OkrCheckInModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            // Truy vấn MongoDB dựa trên giá trị của trường "okr" và "checkin" trong đối tượng "model"
            var filter = Builders<OkrCheckInModel>.Filter.And(
                Builders<OkrCheckInModel>.Filter.Eq(x => x.okr, model.okr),
            Builders<OkrCheckInModel>.Filter.Eq(x => x.date_create, model.date_create)

            );

            var result = await collection.FindAsync(filter).Result.FirstOrDefaultAsync();

            return result;
        }
        // get ra list okr theo thgian tạo và okr
        public static async Task<List<OkrCheckInModel>> GetTheoTimeOutList(OkrCheckInModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            try
            {
                var filter = Builders<OkrCheckInModel>.Filter.And(
                    Builders<OkrCheckInModel>.Filter.Eq(x => x.okr, model.okr),
                    Builders<OkrCheckInModel>.Filter.Eq(x => x.date_create, model.date_create)
                );

                var result = await collection.Find(filter).ToListAsync();

                if (result == null || result.Count == 0)
                {
                    // Log thông báo khi không tìm thấy phần tử
                    Console.WriteLine($"Không tìm thấy phần tử với okr: {model.okr} và date_checkin: {model.date_create}");
                }
                else
                {
                    // Log số lượng phần tử khi tìm thấy
                    //Console.WriteLine($"Đã tìm thấy {result.Count} phần tử với okr: {model.okr} và date_create: {model.date_create}");
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log lỗi
                Console.WriteLine($"Error in GetTheoTime: {ex.Message}");
                return null;
            }
        }
        // get list từ idOKR
        public static async Task<List<OkrCheckInModel>> GetListById(string OkrID)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            var result = await collection.FindAsync(x => x.okr == OkrID).Result.ToListAsync();

            return result;
        }

        //get về từ đúng 1 cái okr
        public static async Task<OkrCheckInModel> GetByOkrID(string OkrID)
        {
            var _db = MongoDb.DbConnect("OKR_Ken"); // Thay thế "your_database_name" bằng tên của cơ sở dữ liệu MongoDB

            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin"); // Thay thế "your_collection_name" bằng tên của collection

            // Truy vấn MongoDB dựa trên giá trị của thuộc tính "id" trong đối tượng "model"
            var result = await collection.FindAsync(x => x.okr == OkrID).Result.ToListAsync();

            return result.LastOrDefault();
        }
         //Xoá okr checkin 
        public static async Task Delete(string id)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            // Tạo bộ lọc để xác định đối tượng cần xóa dựa trên giá trị của thuộc tính "id"
            var filter = Builders<OkrCheckInModel>.Filter.Eq(x => x.id, id);

            // Thực hiện xóa
            var result = await collection.DeleteOneAsync(filter);

            // Kiểm tra xem có bản ghi nào được xóa không
            if (result.DeletedCount > 0)
            {
                Console.WriteLine($"Đã xóa thành công {result.DeletedCount} bản ghi");
            }
            else
            {
                Console.WriteLine("Không có bản ghi nào được xóa");
            }
        }
        //Set trạng thái checkin cho okrcheckin
        public static async Task SetCheckIn(OkrCheckInModel okrset)
        {
            // Kết nối đến cơ sở dữ liệu MongoDB và lấy collection
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            // Tìm và thay thế dựa trên giá trị của thuộc tính "id"
            var filter = Builders<OkrCheckInModel>.Filter.Eq(x => x.id, okrset.id);

            // Tạo một cập nhật để chỉ cập nhật thuộc tính "cycle"
            var update = Builders<OkrCheckInModel>.Update
                .Set(x => x.checkin, true);

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
        // set feedbacks
        public static async Task SetFeedbacks(OkrCheckInModel okrset)
        {
            // Kết nối đến cơ sở dữ liệu MongoDB và lấy collection
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            // Tìm và thay thế dựa trên giá trị của thuộc tính "id"
            var filter = Builders<OkrCheckInModel>.Filter.Eq(x => x.id, okrset.id);

            // Tạo một cập nhật để chỉ cập nhật thuộc tính "cycle"
            var update = Builders<OkrCheckInModel>.Update
                .Set(x => x.feedbacks, okrset.feedbacks);

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
        // lấy okr cũ trước đó 
        public static async Task<OkrCheckInModel> GetOkrCheckinEx(OkrCheckInModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken");
            var collection = _db.GetCollection<OkrCheckInModel>("okrCheckin");

            // Tạo filter cho trường "okr" và "daft"
            var filter = Builders<OkrCheckInModel>.Filter.And(
                Builders<OkrCheckInModel>.Filter.Eq(x => x.okr, model.okr),
                Builders<OkrCheckInModel>.Filter.Eq(x => x.draft, true) // Thêm điều kiện daft == true
            );

            // Sắp xếp giảm dần theo trường "date_create"
            var sort = Builders<OkrCheckInModel>.Sort.Descending(x => x.date_create);

            // Thực hiện truy vấn và lấy phần tử đầu tiên
            var result = await collection.Find(filter).Sort(sort).FirstOrDefaultAsync();

            // Kiểm tra và log kết quả
            if (result != null)
            {
                Console.WriteLine("Đã tìm thấy phần tử:");
                Console.WriteLine($"Id: {result.id}, Date_Create: {result.date_create}, Daft: {result.draft}");
            }
            else
            {
                Console.WriteLine("Không tìm thấy phần tử.");
            }

            return result;
        }
    }
}
