//namespace MyPhotoshop
//{
//    public class GrayscaleFilter : PixelFilter<GrayscaleParameters>
//    {
//        public override string ToString() => "Оттенки серого";

//        protected override Pixel ProcessPixel(Pixel original, GrayscaleParameters parameters)
//        {
//            var average = 0.3 * original.R + 0.587 * original.G + 0.114 * original.B;
//            var coef = (parameters as GrayscaleParameters).Coefficient;

//            return new Pixel(
//                Pixel.Trim(original.R * (1 - coef) + average * coef),
//                Pixel.Trim(original.G * (1 - coef) + average * coef),
//                Pixel.Trim(original.B * (1 - coef) + average * coef));
//        }
//    }
//}