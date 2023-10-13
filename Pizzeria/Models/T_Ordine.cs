namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_Ordine()
        {
            T_Dettaglio = new HashSet<T_Dettaglio>();
        }

        [Key]
        public int IdOrdine { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataOrdine { get; set; }

        public int FKUtente { get; set; }

        [Required]
        [StringLength(50)]
        public string Indirizzo { get; set; }

        public bool? Evaso { get; set; }

        public string Nota { get; set; }

        [Column(TypeName = "money")]
        public decimal? Totale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_Dettaglio> T_Dettaglio { get; set; }

        public virtual T_Utenti T_Utenti { get; set; }
    }
}
