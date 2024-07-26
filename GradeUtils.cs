// This is just a utility class to calculate the total marks - 
// to be used by the GradeWiz and ExportToExcel classes.

using System.Collections.Generic;

namespace GradeWiz
{
    public static class GradeUtils
    {
        public static double CalculateTotalMark(Dictionary<int, (double Weighting, string Name)> componentDetails, double[] scores)
        {
            double totalMark = 0;
            foreach (var component in componentDetails)
            {
                var (weighting, _) = component.Value;
                int index = component.Key - 1;
                totalMark += scores[index] * (weighting / 100);
            }
            return totalMark;
        }
    }
}
