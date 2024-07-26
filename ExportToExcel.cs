using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class ExportToExcel
{
    private string _moduleCode;
    private Dictionary<int, (double Weighting, string Name)> _componentDetails;
    private double[] _scores;

    public ExportToExcel(string moduleCode, Dictionary<int, (double Weighting, string Name)> componentDetails, double[] scores)
    {
        _moduleCode = moduleCode;
        _componentDetails = componentDetails;
        _scores = scores;
    }

    public void Export()
    {
        using (var sfd = new SaveFileDialog())
        {
            sfd.Filter = "Excel Files|*.xlsx";
            sfd.Title = "Save Results As";
            sfd.FileName = "ModuleResult.xlsx"; // Default file name

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfd.FileName;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Module Result");

                    // Add module code as a header
                    worksheet.Cells[1, 1].Value = $"Module Code: {_moduleCode.ToUpper()}";
                    worksheet.Cells[1, 1, 1, 4].Merge = true; // Merge across columns
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 14;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Add headers
                    worksheet.Cells[2, 1].Value = "Component";
                    worksheet.Cells[2, 2].Value = "Name";
                    worksheet.Cells[2, 3].Value = "Weight";
                    worksheet.Cells[2, 4].Value = "Mark";

                    // Format headers
                    worksheet.Cells[2, 1, 2, 4].Style.Font.Bold = true;
                    worksheet.Cells[2, 1, 2, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[2, 1, 2, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[2, 1, 2, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[2, 1, 2, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    // Add data
                    int row = 3;
                    for (int i = 0; i < _componentDetails.Count; i++)
                    {
                        int componentNumber = i + 1;
                        var (weighting, name) = _componentDetails[componentNumber];

                        worksheet.Cells[row, 1].Value = $"#00{componentNumber}";
                        worksheet.Cells[row, 2].Value = name;
                        worksheet.Cells[row, 3].Value = $"{weighting}%";
                        worksheet.Cells[row, 4].Value = _scores[i];
                        worksheet.Cells[row, 4].Style.Font.Bold = true;

                        // Format data rows
                        worksheet.Cells[row, 1, row, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheet.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        row++;
                    }

                    // Add total mark
                    worksheet.Cells[row, 1].Value = "Total Mark";
                    worksheet.Cells[row, 2].Value = CalculateTotalMark(_scores);
                    worksheet.Cells[row, 2].Style.Font.Bold = true;

                    // Format total mark
                    worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
                    worksheet.Cells[row, 1, row, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    // Set column widths
                    worksheet.Column(1).Width = 11.14; // Component Number column width
                    worksheet.Column(2).Width = 40; // Component Name column width
                    worksheet.Column(3).Width = 8; // Weighting column width
                    worksheet.Column(4).Width = 8; // Mark column width

                    // Save the file
                    File.WriteAllBytes(filePath, package.GetAsByteArray());

                    MessageBox.Show($"Results exported to {filePath}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

    private double CalculateTotalMark(double[] scores)
    {
        double totalMark = 0;
        foreach (var component in _componentDetails)
        {
            var (weighting, _) = component.Value;
            int index = component.Key - 1;
            totalMark += scores[index] * (weighting / 100);
        }
        return totalMark;
    }
}
