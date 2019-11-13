using System;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

// <Custom "using" statements>

using KangarooSolver;
using KangarooSolver.Goals;

// </Custom "using" statements>


#region padding (this ensures the line number of this file match with those in the code editor of the C# Script component
















#endregion

public partial class MyExternalScriptBAKE : GH_ScriptInstance
{
    #region Do_not_modify_this_region
    private void Print(string text) { }
    private void Print(string format, params object[] args) { }
    private void Reflect(object obj) { }
    private void Reflect(object obj, string methodName) { }
    public override void InvokeRunScript(IGH_Component owner, object rhinoDocument, int iteration, List<object> inputs, IGH_DataAccess DA) { }
    public RhinoDoc RhinoDocument;
    public GH_Document GrasshopperDocument;
    public IGH_Component Component;
    public int Iteration;
    #endregion


    private void BAKE_EXPORT(List<Curve> x, Line dim, String path, ref object A)
    {
        // <Custom code>


        List<Guid> guidList = new List<Guid>();

        Rhino.DocObjects.Tables.ObjectTable ot = Rhino.RhinoDoc.ActiveDoc.Objects;
        for (int i = 0; i < x.Count; i++)
        {
            if (x[i] == null || !x[i].IsValid)
            {
                RhinoApp.WriteLine("Balls");
                return;
            }
            var crvAtt = new Rhino.DocObjects.ObjectAttributes();
            crvAtt.LayerIndex = 0;
            guidList.Add(ot.AddCurve(x[i], crvAtt));
        }

        Plane dimBase = new Plane(dim.To, dim.From, new Point3d(100000, 100000, 0));
        double ang = Vector3d.VectorAngle(new Vector3d(dim.To - dim.From), Vector3d.XAxis, Plane.WorldXY);
        Print("angle = {0}", ang);
        //dimBase.Rotate(ang, Vector3d.ZAxis);

        var xform = Transform.PlaneToPlane(dimBase, Plane.WorldXY);
        var xformBack = Transform.PlaneToPlane(Plane.WorldXY, dimBase);

        Plane textbase = Plane.WorldXY;
        textbase.Origin = dim.From + new Point3d(-50, -30, 0);
        dim.Transform(xform);


        //Rhino.DocObjects.Tables.DimStyleTable dt = Rhino.RhinoDoc.ActiveDoc.DimStyles;

        var dimension = new LinearDimension(Plane.WorldXY, (new Point2d(dim.From.X, dim.From.Y)), (new Point2d(dim.To.X, dim.To.Y)), (new Point2d(dim.From.X - 100, dim.From.Y - 40)));

        dimension.TextPosition = new Point2d(50, -40);

        dimension.Transform(xformBack);
        dimension.TextHeight = 30;
        dimension.TextAngleType = Rhino.DocObjects.DimensionStyle.LeaderContentAngleStyle.Horizontal;
        var dimAtt = new Rhino.DocObjects.ObjectAttributes();
        dimAtt.LayerIndex = 1;
        guidList.Add(ot.AddLinearDimension(dimension, dimAtt));

        guidList.Add(ot.AddText((dim.Length.ToString("F1")), textbase, 1, "helvetica", false, false, dimAtt));

        int nSelected = ot.Select(guidList);
        if (nSelected != guidList.Count)
        {
            RhinoApp.WriteLine("More Balls");
            return;
        }
        string cmd = "-_Export " + path + " _Enter";
        Print(cmd);
        Rhino.RhinoApp.RunScript(cmd, false);

        ot.Delete(guidList, true);

        A = dimension;

        // </Custom code>
    }

    // <Custom additional code>


    // </Custom additional code>
}
