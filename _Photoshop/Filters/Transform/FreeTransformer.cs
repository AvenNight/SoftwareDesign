using System;
using System.Drawing;

namespace MyPhotoshop
{
    public class FreeTransformer : ITransformer<EmptyParameters>
    {
        private Func<Size, Size> sizeTransformer;
        private Func<Point, Size, Point> pointTransformer;
        private Size originalSize;
        public Size ResultSize { get; private set; }
        
        public FreeTransformer(Func<Size, Size> sizeTransformer, Func<Point, Size, Point> pointTransformer)
        {
            this.sizeTransformer = sizeTransformer;
            this.pointTransformer = pointTransformer;
        }

        public void Prepere(Size oldSize, EmptyParameters @params)
        {
            originalSize = oldSize;
            ResultSize = sizeTransformer(oldSize);
        }

        public Point? MapPoint(Point oldPoint) => pointTransformer(oldPoint, originalSize);
    }
}