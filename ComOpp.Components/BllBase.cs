using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class BllBase
    {
        public BllBase(string tablename)
        {
            this._tablename = tablename;
        }

        private string _tablename = "";

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query">需带where关键字</param>
        public void MoveUp(int id, string query = "")
        {
            CommonDataProvider.Instance().MoveUp(_tablename, id, query);
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query">需带where关键字</param>
        public void MoveDown(int id, string query = "")
        {
            CommonDataProvider.Instance().MoveDown(_tablename, id, query);
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query">需带where关键字</param>
        public void MoveTop(int id, string query = "")
        {
            CommonDataProvider.Instance().MoveTop(_tablename, id, query);
        }
    }
}
