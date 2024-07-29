using GH_IO.Serialization;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetObjectOnCanvas
{
    public class GetObjectOnCanvas_Component : GH_Component
    {
        private HashSet<Guid> m_ids;

        internal HashSet<Guid> ConnectedIds => m_ids;

        public GetObjectOnCanvas_Component()
             : base("Get Object On Canvas", "Get",
              "Get objects, including components, params, groups, scribbles, and so on, by the custom wire connection.",
              "Params", "Util")
        {
            m_ids = new HashSet<Guid>();
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "O", "Objects connected to this component.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetDataList("Objects", ConnectedIds.Select(id => OnPingDocument().FindObject(id, true)));
        }

        #region Wiring ids setting
        public override void AddedToDocument(GH_Document document)
        {
            document.ObjectsDeleted += Document_ObjectsDeleted;
            base.AddedToDocument(document);
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            document.ObjectsDeleted -= Document_ObjectsDeleted;
            base.RemovedFromDocument(document);
        }

        private void Document_ObjectsDeleted(object sender, GH_DocObjectEventArgs e)
        {
            foreach (IGH_Attributes att in e.Attributes)
            {
                if (ConnectedIds.Contains(att.InstanceGuid))
                    ConnectedIds.Remove(att.InstanceGuid);
            }
        }
        #endregion

        #region Serialization
        public override bool Write(GH_IWriter writer)
        {
            GH_IWriter gH_IWriter = writer.CreateChunk("Get Object On Canvas");
            gH_IWriter.SetInt32("id_count", ConnectedIds.Count);
            for (int i = 0; i < ConnectedIds.Count; i++)
            {
                gH_IWriter.SetGuid("id", i, ConnectedIds.ElementAt(i));
            }

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            ConnectedIds.Clear();
            GH_IReader gH_IReader = reader.FindChunk("Get Object On Canvas");
            for (int i = 0; i < gH_IReader.GetInt32("id_count"); i++)
            {
                ConnectedIds.Add(gH_IReader.GetGuid("id", i));
            }

            return base.Read(reader);
        }
        #endregion

        public override void CreateAttributes()
        {
            m_attributes = new GetObjectOnCanvas_Attributes(this);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("A2CF9425-5416-4AB5-AF77-1BDBFF72A4F6");

        public override TimeSpan ProcessorTime => TimeSpan.Zero;
    }
}