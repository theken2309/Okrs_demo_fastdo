using MongoDB.Driver;
using OKRs_Fastdo.Models;

namespace OKRs_Fastdo.Data
{
    public class DbOkrReview
    {
        //kết nối bới collection okr trong DB OKR_Ken
        private static IMongoDatabase _data = MongoDb.DbConnect("OKR_Ken");
        private static IMongoCollection<Okr_reviewModel> _OkrReviewTable = _data.GetCollection<Okr_reviewModel>("okrReview");

        public static Task<List<Okr_reviewModel>> GetAll()
        {
            // Lấy tất cả người dùng từ cơ sở dữ liệu và trả về một danh sách
            return _OkrReviewTable.FindAsync(x => true).Result.ToListAsync();
        }
        public static string CreateOrUpdate(Okr_reviewModel okrrv)
        {
            try
            {
                var filter = Builders<Okr_reviewModel>.Filter.Eq(x => x.okrID, okrrv.okrID);

                var update = Builders<Okr_reviewModel>.Update
                 .Set(x => x.staff_comment, okrrv.staff_comment)
                 .Set(x => x.staff_point, okrrv.staff_point)
                 .Set(x => x.review_staff_comment, okrrv.review_staff_comment)
                 .Set(x => x.review_send_date, okrrv.review_send_date)
                 .Set(x => x.review_manager_comment, okrrv.review_manager_comment)
                 .Set(x => x.manager_point, okrrv.manager_point)
                 .Set(x => x.review_send_status, okrrv.review_send_status)
                 .SetOnInsert(x => x.id, MongoDb.RandomId()); // RandomId() sẽ tạo một giá trị ngẫu nhiên khi tạo mới

                //Thêm các trường cần cập nhật ở đây

                var result = _OkrReviewTable.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });

                if (result.IsAcknowledged)
                {
                    if (result.MatchedCount > 0)
                    {
                        return "Updated successfully";
                    }
                    else
                    {
                        return "Created successfully";
                    }
                }
                else
                {
                    return "Operation failed";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An lz luon");
                return $"Error creating/updating record: {ex.Message}";
            }
        }

        public static string Update(Okr_reviewModel updatedOkrReview)
        {
            try
            {
                // Tìm đối tượng cần cập nhật dựa trên ID
                var filter = Builders<Okr_reviewModel>.Filter.Eq(x => x.id, updatedOkrReview.id);

                // Tạo một đối tượng cập nhật
                var update = Builders<Okr_reviewModel>.Update
                    .Set(x => x.review_staff_comment, updatedOkrReview.review_staff_comment)
                    .Set(x => x.staff_comment, updatedOkrReview.staff_comment)
                    .Set(x => x.manager_comment, updatedOkrReview.manager_comment)
                    .Set(x => x.review_send_status, updatedOkrReview.review_send_status)
                    .Set(x => x.review_manager_comment, updatedOkrReview.review_manager_comment);
                //Thêm các trường cần cập nhật ở đây

                // Thực hiện cập nhật
                var result = _OkrReviewTable.UpdateOne(filter, update);

                if (result.ModifiedCount == 1)
                {
                    return "Updated successfully";
                }
                else
                {
                    return "Record not found or not updated";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An lz luon");
                return $"Error updating record: {ex.Message}";
            }
        }
        public static string Delete(string okrId)
        {
            try
            {
                var filter = Builders<Okr_reviewModel>.Filter.Eq(x => x.okrID, okrId);

                var result = _OkrReviewTable.DeleteOne(filter);

                if (result.IsAcknowledged && result.DeletedCount > 0)
                {
                    Console.WriteLine($"Đã xóa thành công: {okrId}");
                    return "Xóa thành công";
                }
                else if (result.IsAcknowledged && result.DeletedCount == 0)
                {
                    Console.WriteLine($"Không tìm thấy bản ghi để xóa: {okrId}");
                    return "Không tìm thấy bản ghi để xóa";
                }
                else
                {
                    Console.WriteLine($"Xóa thất bại cho {okrId}. Lỗi: ");
                    return "Xóa thất bại";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa bản ghi {okrId}: {ex.Message}");
                return $"Lỗi khi xóa bản ghi: {ex.Message}";
            }
        }

        public static async Task<Okr_reviewModel> Getbyokrid(Okr_reviewModel model)
        {
            var _db = MongoDb.DbConnect("OKR_Ken"); // Thay thế "your_database_name" bằng tên của cơ sở dữ liệu MongoDB

            var collection = _db.GetCollection<Okr_reviewModel>("okrReview"); // Thay thế "your_collection_name" bằng tên của collection

            // Truy vấn MongoDB dựa trên giá trị của thuộc tính "id" trong đối tượng "model"
            var result = await collection.FindAsync(x => x.okrID == model.okrID).Result.ToListAsync();

            return result.LastOrDefault();
        }


    }
}
