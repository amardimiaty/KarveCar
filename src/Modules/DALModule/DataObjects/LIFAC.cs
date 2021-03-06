using System;
using KarveDapper;
using KarveDapper.Extensions;

namespace DataAccessLayer.DataObjects
{
    /// <summary>
    /// Represents a LIFAC.
    /// NOTE: This class is generated from a T4 template - you should not modify it manually.
    /// </summary>
    [Table("LIFAC")]
    public class LIFAC 
	{
	
	/// <summary>
    ///  Set or get the CONCEPTO_LIF property.
    /// </summary>
    
		public Int32? CONCEPTO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the DESCRIP_LIF property.
    /// </summary>
    
		public string DESCRIP_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the DTO_LIF property.
    /// </summary>
    
		public Decimal? DTO_LIF { get; set; }
 
	
	/// <summary>
    ///  Set or get the ULTMODI_LIF property.
    /// </summary>
    
		public string ULTMODI_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the UNIDAD_LIF property.
    /// </summary>
    
		public string UNIDAD_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the USUARIO_LIF property.
    /// </summary>
    
		public string USUARIO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the VEHICULO_LIF property.
    /// </summary>
    
		public string VEHICULO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the CONTRATO_LIF property.
    /// </summary>
    
		public string CONTRATO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the NUMERO_LIF property.
    /// </summary>
    
		public string NUMERO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the CANTIDAD_LIF property.
    /// </summary>
    
		public Decimal? CANTIDAD_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the SUBTOTAL_LIF property.
    /// </summary>
    
		public Decimal? SUBTOTAL_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the DTO_SN property.
    /// </summary>
    
		public string DTO_SN { get; set; }
 
	/// <summary>
    ///  Set or get the MONEDA_LIF property.
    /// </summary>
    
		public string MONEDA_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the TLL_PRODUCTO_LIF property.
    /// </summary>
    
		public string TLL_PRODUCTO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the IVA property.
    /// </summary>
    
		public Decimal? IVA { get; set; }
 
	/// <summary>
    ///  Set or get the LINEA property.
    /// </summary>
    
		public Int32? LINEA { get; set; }
 
	/// <summary>
    ///  Set or get the CLAVE_LF property.
    /// </summary>
        [Key]
		public Int32 CLAVE_LF { get; set; }
 
	/// <summary>
    ///  Set or get the FAC_REC_DIRECTA_LF property.
    /// </summary>
    
		public byte? FAC_REC_DIRECTA_LF { get; set; }
 
	/// <summary>
    ///  Set or get the DPT_LF property.
    /// </summary>
    
		public string DPT_LF { get; set; }
 
	/// <summary>
    ///  Set or get the DIAS property.
    /// </summary>
    
		public Int32? DIAS { get; set; }
 
	/// <summary>
    ///  Set or get the IMPORTE_ANULADA property.
    /// </summary>
    
		public Decimal? IMPORTE_ANULADA { get; set; }
 
	/// <summary>
    ///  Set or get the CUOTA_LIF property.
    /// </summary>
    
		public Decimal? CUOTA_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the SUPLIDO property.
    /// </summary>
    
		public Decimal? SUPLIDO { get; set; }
 
	/// <summary>
    ///  Set or get the KM_TOTAL property.
    /// </summary>
    
		public Decimal? KM_TOTAL { get; set; }
 
	/// <summary>
    ///  Set or get the KMCONTRA property.
    /// </summary>
    
		public Decimal? KMCONTRA { get; set; }
 
	/// <summary>
    ///  Set or get the CUOTA property.
    /// </summary>
    
		public Int32? CUOTA { get; set; }
 
	/// <summary>
    ///  Set or get the SERIECUOTA_LIF property.
    /// </summary>
    
		public byte? SERIECUOTA_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the FECSERV_LIF property.
    /// </summary>
    
		public DateTime? FECSERV_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the SALA_LIF property.
    /// </summary>
    
		public Int32? SALA_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the ENKIT property.
    /// </summary>
    
		public string ENKIT { get; set; }
 
	/// <summary>
    ///  Set or get the DESDE_LIF property.
    /// </summary>
    
		public DateTime? DESDE_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the HASTA_LIF property.
    /// </summary>
    
		public DateTime? HASTA_LIF { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? PRE_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the NO_RECARGO_LIF property.
    /// </summary>
    
		public byte? NO_RECARGO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the TIOPE_LIF property.
    /// </summary>
    
		public string TIOPE_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the CLAVE_LICOMP property.
    /// </summary>
    
		public Int32? CLAVE_LICOMP { get; set; }
 
	/// <summary>
    ///  Set or get the SALTAPAG property.
    /// </summary>
    
		public byte? SALTAPAG { get; set; }
 
	/// <summary>
    ///  Set or get the ACTIVEHI_LF property.
    /// </summary>
    
		public string ACTIVEHI_LF { get; set; }
 
	/// <summary>
    ///  Set or get the COSTE property.
    /// </summary>
    
		public Decimal? COSTE { get; set; }
 
	/// <summary>
    ///  Set or get the DOCUSAP_LIF property.
    /// </summary>
    
		public string DOCUSAP_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the EXPEDIENTE_LIF property.
    /// </summary>
    
		public string EXPEDIENTE_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the INFO_FACE property.
    /// </summary>
    
		public string INFO_FACE { get; set; }
 
	/// <summary>
    ///  Set or get the COMENTARIO_LIF property.
    /// </summary>
    
		public string COMENTARIO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the DESC_ADD_LIF property.
    /// </summary>
    
		public string DESC_ADD_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the ANALITICA1 property.
    /// </summary>
    
		public string ANALITICA1 { get; set; }
 
	/// <summary>
    ///  Set or get the ANALITICA2 property.
    /// </summary>
    
		public string ANALITICA2 { get; set; }
 
	/// <summary>
    ///  Set or get the ES_DESCRIPTIVO_LIF property.
    /// </summary>
    
		public byte? ES_DESCRIPTIVO_LIF { get; set; }
 
	/// <summary>
    ///  Set or get the FAC_ABONO_RECUPERA_LF property.
    /// </summary>
    
		public string FAC_ABONO_RECUPERA_LF { get; set; }
	}
}
