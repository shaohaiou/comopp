using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class CustomerConnectRecordQuery : IQuery
    {
        private string _column = "*";
        private string _tableName = "ComOpp_CustomerConnectRecord";
        private string _orderby = " ID desc";

        #region IQuery 成员

        /// <summary>
        /// 需要返回的列
        /// </summary>
        public string Column
        {
            get
            {
                return _column;
            }
            set
            {
                _column = value;
            }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy
        {
            get
            {
                return _orderby;
            }
            set
            {
                _orderby = value;
            }
        }
        public int CorporationID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int ConnectUserID { get; set; }
        public int ConnectwayID { get; set; }
        public int CustomerLevelID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 生成where
        /// </summary>
        /// <returns></returns>
        public string BulidQuery()
        {
            List<string> query = new List<string>();
            if (CorporationID > 0)
            {
                query.Add(string.Format("[CorporationID]='{0}'", CorporationID));
            }
            if (!string.IsNullOrEmpty(CustomerName))
            {
                query.Add(string.Format("[CustomerName]='{0}'", CustomerName));
            }
            if (!string.IsNullOrEmpty(CustomerPhone))
            {
                query.Add(string.Format("[CustomerPhone]='{0}'", CustomerPhone));
            }
            if (ConnectUserID > 0)
            {
                query.Add(string.Format("[ConnectUserID]='{0}'", ConnectUserID));
            }
            if (ConnectwayID > 0)
            {
                query.Add(string.Format("[ConnectwayID]='{0}'", ConnectwayID));
            }
            if (CustomerLevelID > 0)
            {
                query.Add(string.Format("[CustomerLevelID]='{0}'", CustomerLevelID));
            }
            if (StartTime.HasValue)
            {
                query.Add(string.Format("[CreateTime] > '{0}'", StartTime.Value.ToString("yyyy-MM-dd HH:mm")));
            }
            if (EndTime.HasValue)
            {
                query.Add(string.Format("[CreateTime] < '{0}' ", EndTime.Value.ToString("yyyy-MM-dd HH:mm")));
            }
            return string.Join(" AND ", query);

        }

        /// <summary>
        /// 生成sql
        /// </summary>
        /// <returns></returns>
        public string BulidSelect(string where, string tableName = "")
        {
            return string.Empty;
        }
        #endregion
    }
}
