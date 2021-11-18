using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class RolesControl
    {
        [Key]
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string EmailAdress { get; set; }
    }
}
