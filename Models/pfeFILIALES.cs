//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pfeAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class pfeFILIALES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pfeFILIALES()
        {
            this.pfeFACTURAS = new HashSet<pfeFACTURAS>();
            this.pfeORDENCOMPRA = new HashSet<pfeORDENCOMPRA>();
        }
    
        public decimal IDEMPRESA { get; set; }
        public decimal IDFILIAL { get; set; }
        public string RAZONSOCIAL { get; set; }
        public string RFC { get; set; }
        public string EMAILREVISION { get; set; }
        public string EMAILPAGOS { get; set; }
        public string EMAILCONTACTO { get; set; }
        public string TELEFONO { get; set; }
        public string IDUSUARIO { get; set; }
    
        public virtual pfeEMPRESA pfeEMPRESA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pfeFACTURAS> pfeFACTURAS { get; set; }
        public virtual pfeUSUARIOS pfeUSUARIOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pfeORDENCOMPRA> pfeORDENCOMPRA { get; set; }
    }
}
