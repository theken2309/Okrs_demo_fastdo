using MongoDB.Bson.Serialization.Attributes;

namespace OKRs_Fastdo.Models
{
    public class Okr_reviewModel
    {
        [BsonId]
        public string id { get; set; }
        
  
        public string okrID { get; set; }
        /// <summary>Nhân sự tự đánh giá</summary>

        public double staff_point { get; set; }

        /// <summary>Quản lý đánh giá</summary>
        public double manager_point { get; set; }

        /// <summary>Nhân sự tự nhận xét</summary>
        public string staff_comment { get; set; }

        /// <summary>Quản lý nhận xét</summary>
        public string manager_comment { get; set; }
        /// <summary>Thời gian nhân viên gửi đánh giá</summary>
        public long review_send_date { get; set; }

        /// <summary>Trạng thái gửi đánh giá: 1. Nháp, 2. Đã gửi, 3. Đã xác nhận</summary>
        public int review_send_status { get; set; }

        /// <summary>Nhân sự tự nhận xét</summary>
        public string review_staff_comment { get; set; }

        /// <summary>Quản lý nhận xét</summary>
        public string review_manager_comment { get; set; }

        /// <summary>Quản lý đánh giá: ID</summary>
        public string review_manager_id { get; set; }

        /// <summary>Quản lý đánh giá: thời gian</summary>
        public long review_manager_date { get; set; }

        /// <summary>Người xem đánh giá</summary>
        public List<string> review_viewers { get; set; } = new();
    }
}
