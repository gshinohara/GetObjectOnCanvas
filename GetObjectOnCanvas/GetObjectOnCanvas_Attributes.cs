using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GetObjectOnCanvas
{
    internal class GetObjectOnCanvas_Attributes : GH_ComponentAttributes
    {
        internal PointF CustomGrip => new PointF(Bounds.Left + 10, Bounds.Bottom);

        public GetObjectOnCanvas_Attributes(GetObjectOnCanvas_Component owner) : base(owner)
        {
        }        

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            GetObjectOnCanvas_Component thiscomponent = Owner as GetObjectOnCanvas_Component;
            switch (channel)
            {
                case GH_CanvasChannel.First:
                    if (Selected)
                    {
                        foreach (Guid connectedId in thiscomponent.ConnectedIds)
                        {
                            if (thiscomponent.OnPingDocument().FindObject(connectedId, true) is IGH_DocumentObject connectedObj)
                                graphics.DrawPath(new Pen(Color.MediumSeaGreen, 10), GH_CapsuleRenderEngine.CreateRoundedRectangle(connectedObj.Attributes.Bounds, 3));
                        }
                    }
                    break;
                case GH_CanvasChannel.Wires:
                    foreach (Guid connectedId in thiscomponent.ConnectedIds)
                    {
                        if (thiscomponent.OnPingDocument().FindObject(connectedId, true) is IGH_DocumentObject connectedObj)
                            this.DrawCustomWire(connectedObj.Attributes.Bounds.Location, graphics); //Default color
                    }
                    break;
                case GH_CanvasChannel.Objects:
                    GH_CapsuleRenderEngine.RenderInputGrip(graphics, canvas.Viewport.Zoom, CustomGrip, true);
                    break;
                default:
                    break;
            }

            base.Render(canvas, graphics, channel);
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left && GH_GraphicsUtil.Distance(e.CanvasLocation, CustomGrip) <= 10)
            {
                sender.ActiveInteraction = new GetObjectOnCanvas_LinkingInteraction(sender, e, this);
                return GH_ObjectResponse.Handled;
            }

            return base.RespondToMouseDown(sender, e);
        }
    }
}