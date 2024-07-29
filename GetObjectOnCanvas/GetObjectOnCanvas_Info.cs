using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GetObjectOnCanvas
{
    public class GetObjectOnCanvas_Info : GH_AssemblyInfo
    {
        public override string Name => "GetObjectOnCanvas";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("b327aa58-cde0-46fd-aaeb-b08545f93ebd");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}