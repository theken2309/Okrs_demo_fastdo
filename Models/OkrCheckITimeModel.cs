using MongoDB.Bson.Serialization.Attributes;

namespace OKRs_Fastdo.Models
{
    public class OkrCheckITimeModel
    {
        [BsonId]
        public string id { get; set; }

        public long date_checkin { get; set; }
        public string okr { get; set; }
        /// <summary>
        /// trang thai cua check-in 1 nháp , 2 done
        /// </summary>
        public int status { get;set; }
    }
}
