using OBEHR.Models.Base;
using OBEHR.Models.DAL;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models.ViewModels
{
    public class HRAEditClient
    {
        public HRAEditClient()
        {
            WorkCitiesIds = new List<int> { };
            TaxCitiesIds = new List<int> { };
            PensionCitiesIds = new List<int> { };
            AccumulationCitiesIds = new List<int> { };
            HRPPUsersIds = new List<string> { };

        }
        public int Id { get; set; }

        public ICollection<int> WorkCitiesIds { get; set; }
        public ICollection<int> TaxCitiesIds { get; set; }
        public ICollection<int> PensionCitiesIds { get; set; }
        public ICollection<int> AccumulationCitiesIds { get; set; }
        public ICollection<string> HRPPUsersIds { get; set; }
    }
}