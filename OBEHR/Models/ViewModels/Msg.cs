using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models.ViewModels
{
    public class Msg
    {
        public MsgType MsgType { get; set; }
        public string Content { get; set; }
    }
}