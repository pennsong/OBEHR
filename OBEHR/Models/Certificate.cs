﻿using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class Certificate : BaseModel
    {
        public override string ToString()
        {
            return Name;
        }
    }
}