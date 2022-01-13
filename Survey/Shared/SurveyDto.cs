using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Shared
{
    public class SurveyDto
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
        public bool Deleted { get; set; }

        public DateTime InsertedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public int UpdateCount { get; set; }

    }
}
