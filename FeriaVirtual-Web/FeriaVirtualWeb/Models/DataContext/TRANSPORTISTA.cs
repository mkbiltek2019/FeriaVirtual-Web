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

    public partial class TRANSPORTISTA
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public decimal IDTRANSPORTISTA { get; set; }
        public string RUTTRANSPORTISTA { get; set; }
        public string NOMBRE { get; set; }
        public string TELEFONO { get; set; }
        [Display(Name = "TRANSPORTE")]
        public string TIPOTRANSPORTE { get; set; }
        public decimal ANCHO { get; set; }
        public decimal ALTO { get; set; }
        public decimal LARGO { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name ="CAPACIDAD")]
        public decimal CAPACIDADCARGA { get; set; }
        public string REFRIGERACION { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        [Display(Name = "N° SUBASTA")]
        public Nullable<decimal> SUBASTAID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0}")]
        public Nullable<decimal> PRECIO { get; set; }
        public string ESTADOSUBASTA { get; set; }
        [Display(Name = "TRANSPORTE")]
        public List<TRANSPORTISTA> TRANSPORTELISTA { get; set; }
        
        public virtual SUBASTA SUBASTA { get; set; }
    }
}
