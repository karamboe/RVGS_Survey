using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Shared.Models
{
    public class SurveyDto
    {
        public string Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }

        [Required] 
        public bool Deleted { get; set; }

        [Required] 
        public DateTime InsertedDate { get; set; }
        [Required] 
        public string InsertedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        [Required] 
        public int UpdateCount { get; set; }



    }
}
