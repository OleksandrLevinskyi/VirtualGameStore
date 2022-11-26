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
        SetXlsFormatting(ws, data.Count);
        return ws;
    }

    private void SetXlsHeaders(IXLWorksheet ws)
    {
        SetRowValues<string>(ws, 1, ColumnDefinitions.Select(c => c.Label).ToList());
        ws.Row(1).Style.Font.Bold = true;
    }

    private void SetXlsData(IXLWorksheet ws, List<TD> dataItems)
    {
        int i = 1;
        foreach (var item in dataItems)
            SetRowValues<object>(ws, ++i, ColumnDefinitions.Select(c => c.Value(item)).ToList());
    }

    private void SetXlsFormatting(IXLWorksheet ws, int rowCount)
    {
        int len = ColumnDefinitions.Length;
        for (int i = 0; i < len; i++)
        {
            var columnDef = ColumnDefinitions[i];
            var columnRange = ws.Range(2, i + 1, rowCount + 2, i + 1);
            var columnNums = columnRange.Cells();
            if (columnDef.DataType == XLDataType.Number) {
                columnNums = columnRange.Cells(c => IsNumericType(c.Value));
                columnRange.Cells(c => !IsNumericType(c.Value)).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;
            }

            columnNums.DataType = columnDef.DataType;
            if (columnDef.NumberFormat is not null) 
                columnNums.Style.NumberFormat.Format = columnDef.NumberFormat;
        }

        ws.Columns().AdjustToContents();
    }


    private static void SetRowValues<TR>(IXLWorksheet ws, int row, List<TR> values)
    {
        for (var i = 0; i < values.Count; i++)
            ws.Cell(row, i + 1).Value = values[i];
    }
    
    private static bool IsNumericType(object o)
    {
        return Type.GetTypeCode(o.GetType()) switch
        {
            TypeCode.Byte => true,
            TypeCode.SByte => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Single => true,
            _ => false
        };
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