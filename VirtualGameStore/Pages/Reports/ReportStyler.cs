using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace VirtualGameStore.Pages.Reports;

public class ReportStyler<TD>
{
    public readonly Column[] ColumnDefinitions;

    public ReportStyler(Column[] columns)
    {
        ColumnDefinitions = columns;
    }

    public IXLWorksheet AddReportAsSheet(XLWorkbook workbook, string name, List<TD> data)
    {
        var ws = workbook.Worksheets.Add(name);
        SetXlsHeaders(ws);
        SetXlsData(ws, data);
        SetXlsFormatting(ws);
        return ws;
    }

    private void SetXlsHeaders(IXLWorksheet ws)
    {
        SetRowValues<string>(ws, 1, ColumnDefinitions.Select(c => c.Label).ToList());
    }

    private void SetXlsData(IXLWorksheet ws, List<TD> dataItems)
    {
        int i = 1;
        foreach (var item in dataItems)
            SetRowValues<object>(ws, ++i, ColumnDefinitions.Select(c => c.Value(item)).ToList());
    }

    private void SetXlsFormatting(IXLWorksheet ws)
    {
        int len = ColumnDefinitions.Length;
        for (int i = 0; i < len; i++)
        {
            var columnDef = ColumnDefinitions[i];
            var columnRange = ws.Range(2, i + 1, len, i + 1);
            columnRange.DataType = columnDef.DataType;
            if (columnDef.NumberFormat is not null)
                columnRange.Style.NumberFormat.Format = columnDef.NumberFormat;
        }

        ws.Columns().AdjustToContents();
    }


    private static void SetRowValues<TR>(IXLWorksheet ws, int row, List<TR> values)
    {
        for (var i = 0; i < values.Count; i++)
            ws.Cell(row, i + 1).Value = values[i];
    }

    public struct Column
    {
        public readonly string Label;
        public readonly XLDataType DataType;
        public readonly Func<TD, object> Value;
        public readonly string? NumberFormat;

        public Column(string label, XLDataType dataType, Func<TD, object> value, string? numberFormat = null)
        {
            Label = label;
            DataType = dataType;
            Value = value;
            NumberFormat = numberFormat;
        }
    }
}