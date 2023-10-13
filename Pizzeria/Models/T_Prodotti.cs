namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Prodotti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_Prodotti()
        {
            T_Dettaglio = new HashSet<T_Dettaglio>();
        }

        [Key]
        public int IdProdotto { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [Column(TypeName = "money")]
        public decimal? Costo { get; set; }

        public int? TempoConsegna { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        public string Ingredienti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_Dettaglio> T_Dettaglio { get; set; }
    }
}
