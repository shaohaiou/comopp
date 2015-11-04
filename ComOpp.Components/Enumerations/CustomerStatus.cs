using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ComOpp.Components
{
    public enum CustomerStatus
    {
        [Description("导入|集客")]
        导入_集客 = 1,
        [Description("清洗|邀约")]
        清洗_邀约 = 2,
        [Description("到店|洽谈")]
        到店_洽谈 = 3,
        [Description("追踪|促成")]
        追踪_促成 = 4,
        [Description("预订|成交")]
        预订_成交 = 5,
        [Description("提车|回访")]
        提车_回访 = 10,
        [Description("转出待审")]
        转出待审 = 21,
        [Description("潜客(转出)")]
        潜客_转出 = 31,
        [Description("潜客(战败)")]
        潜客_战败 = 32,
        [Description("其他潜客")]
        其他潜客 = 33
    }
}
