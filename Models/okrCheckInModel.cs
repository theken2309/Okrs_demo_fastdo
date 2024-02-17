using MongoDB.Bson.Serialization.Attributes;

namespace OKRs_Fastdo.Models
{
    public class OkrCheckInModel
    {
        [BsonId]
        public string id { get; set; }

        /// <summary>Chu kỳ</summary>
        public string cycle { get; set; }

        /// <summary>Ngày tạo</summary>
        public long date_create { get; set; }

        /// <summary>Người tạo</summary>
        public string user_create { get; set; }

        /// <summary>Ngày hoàn tất check-in</summary>
        public long date_checkin { get; set; }

        /// <summary>Người checkin</summary>
        public string user_checkin { get; set; }

        /// <summary>ID OKRs</summary>
        public string okr { get; set; }

        /// <summary>Độ tự tin</summary>
        public int confident { get; set; }

        /// <summary>Tiến độ lần checkin trước</summary>
        public double progress_prev { get; set; }

        /// <summary>Checkin nháp/Xác nhận</summary>
        public bool draft { get; set; }

        /// <summary>Checkin xong/chưa</summary> 
        public bool checkin { get; set; }

        /// <summary>Checkin Kết quả chính</summary>
        public List<KeyResult> key_results { get; set; } = new();

        /// <summary>Phản hồi checkin</summary>
        public List<Feedback> feedbacks { get; set; } = new();

        public long date_create_okr { get; set; }
        public long review_send_date { get; set; }
        public long review_manager_date { get; set; }

        public class KeyResult
        {
            public string name { get; set; }
            public string id { get; set; }
            public double result { get; set; }
            public double target { get; set; }
            public double change { get; set; }
            public int confident { get; set; }
            public string unit { get; set; }
            public string question1 { get; set; }
            public string question2 { get; set; }
            public string question3 { get; set; }
            public string question4 { get; set; }

            public string feedback { get; set; }

            /// <summary>Nhân sự tự đánh giá</summary>
            public double staff_point { get; set; }

            /// <summary>Quản lý đánh giá</summary>
            public double manager_point { get; set; }

            /// <summary>Nhân sự tự nhận xét</summary>
            public string staff_comment { get; set; }

            /// <summary>Quản lý nhận xét</summary>
            public string manager_comment { get; set; }
        }

        public class Feedback
        {
            public string id { get; set; }
            public string user { get; set; }
            public string content { get; set; }
            public string kr { get; set; }
            public long date { get; set; }
        }
       
    }

}
