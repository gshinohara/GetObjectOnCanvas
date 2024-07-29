using System.Drawing;

namespace GetObjectOnCanvas
{
    internal static class GetObjectOnCanvas_WireUtil
    {
        public static void DrawCustomWire(this GetObjectOnCanvas_Attributes srcAtt, PointF targetPt, Graphics graphics, KnownColor color = KnownColor.MediumSeaGreen)
        {
            Pen pen = new Pen(Color.FromKnownColor(color), 3);
            graphics.DrawBezier(pen, srcAtt.CustomGrip, new PointF(srcAtt.CustomGrip.X, srcAtt.CustomGrip.Y + 50), new PointF(targetPt.X, targetPt.Y - 50), targetPt);
            pen.Dispose();
        }
    }
}