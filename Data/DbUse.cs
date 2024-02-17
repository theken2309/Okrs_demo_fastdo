using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbUse
    {
        //kết nối bới collection okr trong DB OKR_Ken
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<UseModel> _UseTable = _data.GetCollection<UseModel>("Use");

        // get dựa trên id
        public static async Task<UseModel> Get(UseModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken"); // Thay thế "your_database_name" bằng tên của cơ sở dữ liệu MongoDB

            var collection = _db.GetCollection<UseModel>("Use"); // Thay thế "your_collection_name" bằng tên của collection

            // Truy vấn MongoDB dựa trên giá trị của thuộc tính "id" trong đối tượng "model"
            var result = await collection.FindAsync(x => x.id == model.id).Result.FirstOrDefaultAsync();

            return result;
        }

        //lay danh sach cua OKR
        public static Task<List<UseModel>> GetAll()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _UseTable.FindAsync(x => true).Result.ToListAsync();
        }
        public static void Create(UseModel model)
        {
            model.id = MongoDb.RandomId();
            _UseTable.InsertOne(model);
        }
        public static async Task<UseModel> CheckLogin(UseModel model)
        {
            Console.WriteLine("Check tai khoan");
            // Tìm kiếm một tài khoản có nameUse và password tương ứng
            var user = await _UseTable.Find(u => u.nameUse == model.nameUse && u.password == model.password).FirstOrDefaultAsync();
            if(user == null)
            {
                Console.WriteLine("khong co ");
            }
            else
            {
                Console.WriteLine("co use");
            }
            // Nếu tìm thấy tài khoản, trả về true, ngược lại trả về false
            return user;
        }
    }
}
