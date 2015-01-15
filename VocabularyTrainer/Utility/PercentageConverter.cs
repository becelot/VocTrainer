using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VocabularyTrainer
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            double val;
            if (!double.TryParse(parameter.ToString(), System.Globalization.NumberStyles.Float, new CultureInfo("de-DE"), out val))
            {
                return System.Convert.ToDouble(value) *
                   System.Convert.ToDouble(parameter.ToString().Replace(".", ","));
            }
            return System.Convert.ToDouble(value) *
                   System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
