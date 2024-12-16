using System;
using System.ComponentModel;


namespace FNTC.Finansoft.Accounting.DTO.Terceros.ViewModels
{
    public class ProductViewModel
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }


        //  [JqGridColumnSortingName("SupplierId")]
        public string Supplier { get; set; }

        //   [JqGridColumnSortingName("CategoryId")]
        public string Category { get; set; }

        [DisplayName("Quantity Per Unit")]
        //[JqGridColumnAlign(JqGridColumnAligns.Center)]
        //   [JqGridColumnFormatter(jqgri]
        public string QuantityPerUnit { get; set; }

        [DisplayName("Unit Price")]
        // [JqGridColumnAlign(JqGridColumnAligns.Center)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Units In Stock")]
        //  [JqGridColumnAlign(JqGridColumnAligns.Center)]
        //[JqGridColumnFormatter(]
        public short? UnitsInStock { get; set; }
        #endregion

        #region Constructor
        public ProductViewModel()
        { }

        public ProductViewModel(Tercero2DTO product)
        {
            this.Id = (Int32.Parse(product.NIT));
            this.Name = product.NOMBRE;
            this.Supplier = product.APELLIDO1;
            this.Category = product.NOMBRE1;
            this.QuantityPerUnit = product.DIGVER;
            this.UnitPrice = 1;
            this.UnitsInStock = (Int16.Parse(product.NIT));
        }
        #endregion
    }

}
