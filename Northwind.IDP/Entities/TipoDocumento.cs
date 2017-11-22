using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.IDP.Entities
{
    [Table("TipoDocumentos")]
    public class TipoDocumento
    {
        [Key]
        public int DocumentoID { get; set; }
        public string Description { get; set; }
    }
}
