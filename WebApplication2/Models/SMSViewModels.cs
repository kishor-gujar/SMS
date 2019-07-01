using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class SMSViewModel
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string Message { get; set; }
    }

    public class MessageViewModel
    {
        [Required]
        public string Message { get; set; }

        public int[] Ids { get; set; }
    }
}