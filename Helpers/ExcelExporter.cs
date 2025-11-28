using ClosedXML.Excel;

public static class ExcelExporter
{
    public static byte[] ExportCotizacionesToExcel(IEnumerable<Seguros.API.Models.Cotizacion> items)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Cotizaciones");
        ws.Cell(1, 1).Value = "NumeroCotizacion";
        ws.Cell(1, 2).Value = "FechaCotizacion";
        ws.Cell(1, 3).Value = "TipoSeguro";
        ws.Cell(1, 4).Value = "Cliente";
        ws.Cell(1, 5).Value = "Moneda";
        ws.Cell(1, 6).Value = "DescripcionBien";
        ws.Cell(1, 7).Value = "SumaAsegurada";
        ws.Cell(1, 8).Value = "Tasa";
        ws.Cell(1, 9).Value = "PrimaNeta";

        int r = 2;
        foreach (var c in items)
        {
            ws.Cell(r, 1).Value = c.NumeroCotizacion;
            ws.Cell(r, 2).Value = c.FechaCotizacion;
            ws.Cell(r, 3).Value = c.TipoSeguro?.Nombre ?? c.TipoSeguroId.ToString();
            ws.Cell(r, 4).Value = c.Cliente?.Nombre ?? c.ClienteId.ToString();
            ws.Cell(r, 5).Value = c.Moneda;
            ws.Cell(r, 6).Value = c.DescripcionBien;
            ws.Cell(r, 7).Value = c.SumaAsegurada;
            ws.Cell(r, 8).Value = c.Tasa;
            ws.Cell(r, 9).Value = Math.Round(c.SumaAsegurada * (c.Tasa / 100m), 2);
            r++;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
