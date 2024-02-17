using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbOkrConfig
    {
        //kết nối bới collection okr trong DB OKR_Ken
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<okrConfigModel> _OKRconfig = _data.GetCollection<okrConfigModel>("okr_config");
        //lay danh sach cua OKR
        public static Task<List<okrConfigModel>> GetAll()
        {
        // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _OKRconfig.FindAsync(x => true).Result.ToListAsync();
        }
        public static async Task<List<okrConfigModel.Cycle>> GetAllCyc()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            var result = await _OKRconfig.Find(x => true).ToListAsync();
            return result.SelectMany(o => o.cycles).ToList();
        }
        public static void Create(okrConfigModel model)
        {
            Console.WriteLine("oke");
            model.id = MongoDb.RandomId();
            _OKRconfig.InsertOne(model);
        }
        public static void AddCycleToOkr(okrConfigModel model, okrConfigModel.Cycle newCycle)
        {
            Console.WriteLine("oke");

            // Tạo một _id mới cho Cycle
            newCycle.id = MongoDb.RandomId();

            // Sử dụng $push để thêm Cycle mới vào mảng cycles của okrConfig
            var filter = Builders<okrConfigModel>.Filter.Eq("_id", model.id);
            var update = Builders<okrConfigModel>.Update.Push("cycles", newCycle);

            _OKRconfig.UpdateOne(filter, update);

            Console.WriteLine($"Cycle added to okrConfig with id: {model.id}");
        }
        public static void UpdateCycleInOkr(okrConfigModel okrConfig, string cycleId, okrConfigModel.Cycle updatedCycle)
        {
            Console.WriteLine("Starting UpdateCycleInOkr...");

            // Xác định vị trí của Cycle trong mảng cycles
            int index = okrConfig.cycles.FindIndex(cycle => cycle.id == cycleId);
            Console.WriteLine("id nhan duoc sua la : " + cycleId);
            Console.WriteLine("id nhan duoc sua la : " + okrConfig.id);
            if (index != -1)
            {
                Console.WriteLine("Cycle found at index: " + index);

                // Cập nhật thông tin của Cycle
                okrConfig.cycles[index] = updatedCycle;

                Console.WriteLine("Cycle updated successfully.");

                // Thực hiện cập nhật trong cơ sở dữ liệu
                var filter = Builders<okrConfigModel>.Filter.Eq("_id", okrConfig.id);
                var update = Builders<okrConfigModel>.Update.Set("cycles", okrConfig.cycles);

                try
                {
                    var result = _OKRconfig.UpdateOne(filter, update);

                    if (result.ModifiedCount == 1)
                    {
                        Console.WriteLine("Update successful. Modified count: " + result.ModifiedCount);
                    }
                    else
                    {
                        Console.WriteLine("Update unsuccessful. Modified count: " + result.ModifiedCount);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Update failed with exception: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Cycle not found.");
            }
        }

        public static void DeleteCycleInOkr(okrConfigModel okrConfig, string cycleId)
        {
            Console.WriteLine("Starting DeleteCycleInOkr...");

            // Xác định vị trí của Cycle trong mảng cycles
            int index = okrConfig.cycles.FindIndex(cycle => cycle.id == cycleId);
            Console.WriteLine("id nhận được để xóa là: " + cycleId);

            if (index != -1)
            {
                Console.WriteLine("Cycle found at index: " + index);

                // Thực hiện xoá Cycle khỏi mảng
                okrConfig.cycles.RemoveAt(index);

                Console.WriteLine("Cycle removed successfully.");

                // Thực hiện cập nhật trong cơ sở dữ liệu
                var filter = Builders<okrConfigModel>.Filter.Eq("_id", okrConfig.id);
                var update = Builders<okrConfigModel>.Update.PullFilter("cycles", Builders<okrConfigModel.Cycle>.Filter.Eq("id", cycleId));

                try
                {
                    var result = _OKRconfig.UpdateOne(filter, update);

                    if (result.ModifiedCount == 1)
                    {
                        Console.WriteLine("Delete successful. Modified count: " + result.ModifiedCount);
                    }
                    else
                    {
                        Console.WriteLine("Delete unsuccessful. Modified count: " + result.ModifiedCount);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Delete failed with exception: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Cycle not found.");
            }
        }
        public static async Task<okrConfigModel> Get(okrConfigModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken"); // Thay thế "your_database_name" bằng tên của cơ sở dữ liệu MongoDB

            var collection = _db.GetCollection<okrConfigModel>("okr_config"); // Thay thế "your_collection_name" bằng tên của collection

            // Truy vấn MongoDB dựa trên giá trị của thuộc tính "id" trong đối tượng "model"
            var result = await collection.FindAsync(x => x.id == model.id).Result.FirstOrDefaultAsync();
            Console.WriteLine("get duoc roi !");
            return result;
        }
        public static void AddSug(okrConfigModel model, okrConfigModel.Suggest suggest)
        {
            Console.WriteLine("oke");

            // Tạo một _id mới cho Cycle
            suggest.id = MongoDb.RandomId();

            // Sử dụng $push để thêm Cycle mới vào mảng cycles của okrConfig
            var filter = Builders<okrConfigModel>.Filter.Eq("_id", model.id);
            var update = Builders<okrConfigModel>.Update.Push("suggests", suggest);

            _OKRconfig.UpdateOne(filter, update);

            Console.WriteLine($"suggests added to okrConfig with id: {model.id}");
        }

        public static void DeleteSug(okrConfigModel okrConfig, string suggestID)
        {

            // Xác định vị trí của suggests trong mảng suggests
            int index = okrConfig.suggests.FindIndex(suggests => suggests.id == suggestID);

            if (index != -1)
            {
                Console.WriteLine("suggest found at index: " + index);

                // Thực hiện xoá suggests khỏi mảng
                okrConfig.suggests.RemoveAt(index);

                Console.WriteLine("suggests removed successfully.");

                // Thực hiện cập nhật trong cơ sở dữ liệu
                var filter = Builders<okrConfigModel>.Filter.Eq("_id", okrConfig.id);
                var update = Builders<okrConfigModel>.Update.PullFilter("suggests", Builders<okrConfigModel.Suggest>.Filter.Eq("id", suggestID));

                try
                {
                    var result = _OKRconfig.UpdateOne(filter, update);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Delete failed with exception: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("suggestID not found.");
            }
        }

        public static void UpdateSug(okrConfigModel okrConfig, string suggestID, okrConfigModel.Suggest sugUpdate)
        {
            Console.WriteLine("Starting UpdatesuggestInOkr...");

            // Xác định vị trí của suggest trong mảng suggest
            int index = okrConfig.suggests.FindIndex(suggest => suggest.id == suggestID);
            Console.WriteLine("id nhan duoc sua la : " + suggestID);
            Console.WriteLine("id nhan duoc sua la : " + okrConfig.id);
            if (index != -1)
            {
                Console.WriteLine("suggestID found at index: " + index);

                // Cập nhật thông tin của suggests
                okrConfig.suggests[index] = sugUpdate;

                Console.WriteLine("sug updated successfully.");

                // Thực hiện cập nhật trong cơ sở dữ liệu
                var filter = Builders<okrConfigModel>.Filter.Eq("_id", okrConfig.id);
                var update = Builders<okrConfigModel>.Update.Set("suggests", okrConfig.suggests);

                try
                {
                    var result = _OKRconfig.UpdateOne(filter, update);

                    if (result.ModifiedCount == 1)
                    {
                        Console.WriteLine("Update successful. Modified count: " + result.ModifiedCount);
                    }
                    else
                    {
                        Console.WriteLine("Update unsuccessful. Modified count: " + result.ModifiedCount);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Update failed with exception: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("suggests not found.");
            }
        }


    }
}
