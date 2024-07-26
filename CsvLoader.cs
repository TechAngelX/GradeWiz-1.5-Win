using System;
using System.Collections.Generic;
using System.IO;

namespace GradeWiz
{
    public static class CsvLoader
    {
        public static Dictionary<int, (double Weighting, string Name)> LoadComponentDetails(string filePath, string moduleCode)
        {
            var componentDetails = new Dictionary<int, (double Weighting, string Name)>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var fields = line.Split(',');

                var moduleCodeInFile = fields[0].ToUpper();
                var passedCode = moduleCode.ToUpper();

                if (moduleCodeInFile == passedCode)
                {
                    for (int i = 1; i < fields.Length; i += 2)
                    {
                        if (i + 1 < fields.Length && double.TryParse(fields[i], out double weighting))
                        {
                            componentDetails[i / 2 + 1] = (weighting, fields[i + 1]);
                        }
                    }
                    break;
                }
            }

            return componentDetails;
        }
    }
}
