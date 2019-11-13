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

public partial class MyExternalScript : GH_ScriptInstance
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


    private void RunScript(List<System.Object> x,  bool reset, ref object A, ref object B)
    {
        // <Custom code>

        var PS = new KangarooSolver.PhysicalSystem();
        List<IGoal> MyGoals = new List<IGoal>();
        List<object> geoOut = new List<object>();


        if (reset)
        {
            resetStart = true;
            geoOut.Clear();
            Print("reset");
            frameCount = 0;
        }

        if(!reset && resetStart)
        {
            run = true;
            resetStart = false;
            Print("run");

        }


        //if (run)
        {
            run = false;
            Print("run");

            for (int i = 0; i < x.Count; i++)
            {
                MyGoals.Add((IGoal)x[i]);
            }

            foreach (IGoal G in MyGoals)
            {
                PS.AssignPIndex(G, 0.01);
            }

            for (int i = 0; i < 10000; i++)
            {
                PS.Step(MyGoals, true, 0);
                //Print(i.ToString());

                if (PS.GetvSum() < 0.0000000001)
                {
                    Print(i.ToString());
                    Print("vsum = {0}", PS.GetvSum());

                    geoOut = PS.GetOutput(MyGoals);
                    break;
                }
            }
        }

        A = geoOut;
        B = frameCount;
        frameCount++;

        // </Custom code>
    }

    // <Custom additional code>
    bool run = false;
    bool resetStart = false;
    int frameCount = 0;


    // </Custom additional code>
}
