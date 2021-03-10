using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Highfield.Models
{
    public class Age
    {
        [Display(Name = "User Id")]
        public Guid UserId { get; set; }

        [Display(Name = "Current Age")]
        public int CurrentAge { get; set; }

        [Display(Name = "Age Plus Twenty")]
        public int AgePlusTwenty { get; set; }
        
    }
}
