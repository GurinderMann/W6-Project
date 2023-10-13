using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models
{
    public class Ordine
    {
        public bool? evaso { get; set; }
        public DateTime? data { get; set; }

        public decimal? total { get; set; }
        
    }
}