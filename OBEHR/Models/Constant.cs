using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public enum MsgType
    {
        OK = 10,
        WARN = 20,
        ERROR = 30,
    }

    public enum EmployeeStatus
    {
        新增未通知 = 10,
        新增已通知 = 20,
        新增已填写 = 25,
        在职 = 30,
        已离职 = 40,
    }

    public enum PensionStatus
    {
        新建 = 10,
        转入 = 20,
        转出 = 30,
    }

    public enum AccumulationStatus
    {
        新建 = 10,
        转入 = 20,
        转出 = 30,
    }

    public enum Degree
    {
        初中及以下 = 10,
        高中 = 20,
        大专 = 30,
        本科 = 40,
        硕士 = 50,
        博士 = 60,
    }

    public enum HukouType
    {
        本地城镇 = 10,
        外地城镇 = 20,
        本地农村 = 30,
        外地农村 = 40,
    }

    public enum Marriage
    {
        已婚 = 10,
        未婚 = 20,
        未知 = 30,
    }

    public enum Sex
    {
        男 = 10,
        女 = 20,
    }

    public enum TaxType
    {
        中国 = 10,
        外籍 = 20,
        劳务 = 30,
    }

    public enum QuitType
    {
        主动离职 = 10,
        被动离职 = 20,
    }

}