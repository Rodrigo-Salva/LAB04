using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LAB04.Converters
{
    /// <summary>Convierte EsError (bool) en el color del TextBlock de mensajes: rojo si es error, verde si es éxito.</summary>
    public class BoolAErrorBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => (value is true) ? Brushes.Red : Brushes.Green;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
