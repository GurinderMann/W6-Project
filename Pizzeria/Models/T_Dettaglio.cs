namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Dettaglio
    {
        [Key]
        public int IdDettaglio { get; set; }

        public int FKProdotto { get; set; }

        public int Quantita { get; set; }

        public int FKOrdine { get; set; }

        public virtual T_Ordine T_Ordine { get; set; }

        public virtual T_Prodotti T_Prodotti { get; set; }
    }
}
