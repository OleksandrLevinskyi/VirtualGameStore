using ClosedXML.Excel;
using VirtualGameStore.Pages.Reports;

namespace VirtualGameStore.Test;

public class ReportsTest
{
    [Test]
    public void GenerateReport_ColumnPlacement_ColumnsInRightSpot()
    {
        using var workbook = new XLWorkbook();
        var reportStyler = new ReportStyler<object>(new ReportStyler<object>.Column[]
        {
            new("A1", XLDataType.Text, EmptyString),
            new("B1", XLDataType.Text, EmptyString),
            new("C1", XLDataType.Text, EmptyString),
        });
        var workSheet = reportStyler.AddReportAsSheet(workbook, "x", new List<object>());
        Assert.Multiple(() =>
        {
            Assert.That((string)workSheet.Cell("A1").Value, Is.EqualTo("A1"));
            Assert.That((string)workSheet.Cell("B1").Value, Is.EqualTo("B1"));
            Assert.That((string)workSheet.Cell("C1").Value, Is.EqualTo("C1"));
        });
    }

    [Test]
    public void GenerateReport_ColumnPlacement_NoExtraColumns()
    {
        using var workbook = new XLWorkbook();
        var reportStyler = new ReportStyler<object>(new ReportStyler<object>.Column[]
        {
            new("A1", XLDataType.Text, EmptyString),
            new("B1", XLDataType.Text, EmptyString),
            new("C1", XLDataType.Text, EmptyString),
        });
        var workSheet = reportStyler.AddReportAsSheet(workbook, "x", new List<object>());
        Assert.Multiple(() =>
        {
            Assert.That((string)workSheet.Cell("A2").Value, Is.EqualTo(""));
            Assert.That((string)workSheet.Cell("D1").Value, Is.EqualTo(""));
        });
    }


    [Test]
    public void GenerateReport_RowData_SavesDifferentTypesSuccessfully()
    {
        using var workbook = new XLWorkbook();
        var reportStyler = new ReportStyler<object>(new ReportStyler<object>.Column[]
        {
            new("String", XLDataType.Text, SampleString),
            new("Integer", XLDataType.Number, SampleInt),
            new("Float", XLDataType.Number, SampleFloat),
        });
        var workSheet = reportStyler.AddReportAsSheet(workbook, "x", new List<object>() { new object() });
        Assert.Multiple(() =>
        {
            // All numbers become doubles
            Assert.That(workSheet.Cell("A2").Value.GetType(), Is.EqualTo(typeof(string)));
            Assert.That(workSheet.Cell("B2").Value.GetType(), Is.EqualTo(typeof(double)));
            Assert.That(workSheet.Cell("C2").Value.GetType(), Is.EqualTo(typeof(double)));
        });
    }

    [Test]
    public void GenerateReport_RowData_SavesXLDataTypesCorrectly()
    {
        using var workbook = new XLWorkbook();
        var reportStyler = new ReportStyler<object>(new ReportStyler<object>.Column[]
        {
            new("String", XLDataType.Text, SampleString),
            new("Integer", XLDataType.Number, SampleInt),
            new("Float", XLDataType.Number, EmptyString),
        });
        var workSheet = reportStyler.AddReportAsSheet(workbook, "x", new List<object>() { new object() });
        Assert.Multiple(() =>
        {
            // All numbers become doubles
            Assert.That(workSheet.Cell("A2").DataType, Is.EqualTo(XLDataType.Text));
            Assert.That(workSheet.Cell("B2").DataType, Is.EqualTo(XLDataType.Number));
            Assert.That(workSheet.Cell("C2").DataType, Is.EqualTo(XLDataType.Text)); // Should be text cause empty
        });
    }

    [Test]
    public void GenerateReport_RowData_CorrectNumberOfRows()
    {
        using var workbook = new XLWorkbook();
        var reportStyler = new ReportStyler<object>(new ReportStyler<object>.Column[]
        {
            new("String", XLDataType.Text, SampleString),
        });
        var workSheet = reportStyler.AddReportAsSheet(
            workbook, "x",
            new List<object>(new object[24])
        );
        Assert.Multiple(() =>
        {
            Assert.That(workSheet.Cell("A25").Value, Is.EqualTo(SampleString(new object())));
            Assert.That(workSheet.Cell("A26").Value, Is.EqualTo(""));
        });
    }

    private static string EmptyString(object ignored) => "";
    private static string SampleString(object ignored) => "sample";
    private static object SampleInt(object ignored) => 57;
    private static object SampleFloat(object ignored) => 3.14f;
}