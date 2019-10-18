//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FeriaVirtualWeb.Models.DataContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class PROCESOVENTA
    {
        public PROCESOVENTA()
        {
            this.CLIENTE = new HashSet<CLIENTE>();
            this.PRODUCTO = new HashSet<PRODUCTO>();
            this.VENTA = new HashSet<VENTA>();
        }
    
        public decimal IDPROCESOVENTA { get; set; }
        public System.DateTime FECHA { get; set; }
        public string ESTADO { get; set; }
        public string TIPOPROCESO { get; set; }
        public Nullable<decimal> ORDENID { get; set; }
        public string NOMBRECLIENTE { get; set; }
        public string PAIS { get; set; }

        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
        public virtual ICollection<PRODUCTO> PRODUCTO { get; set; }
        public virtual ICollection<VENTA> VENTA { get; set; }
        public virtual ORDEN ORDEN { get; set; }
    }
}
