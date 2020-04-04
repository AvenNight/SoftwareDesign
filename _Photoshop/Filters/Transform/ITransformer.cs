using System.Drawing;

namespace MyPhotoshop
{
    public interface ITransformer<TParams> where TParams : IParameters, new()
    {
        void Prepere(Size oldSize, TParams @params);
        Size ResultSize { get; }
        Point? MapPoint(Point oldPoint);
    }
}
