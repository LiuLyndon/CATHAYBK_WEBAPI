namespace CATHAYBK_Model.Base
{
    public abstract class BaseEntity
	{
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CRT_Date { get; set; }
        /// <summary>
        /// 建立使用者ID
        /// </summary>
        public int CRT_User_id { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UPD_Date { get; set; }
        /// <summary>
        /// 更新使用者ID
        /// </summary>
        public int UPD_User_id { get; set; }
    }
}

