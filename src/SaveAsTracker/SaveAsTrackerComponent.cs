using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using SaveAsTracker.Properties;

namespace SaveAsTracker
{
  public class SaveAsTrackerComponent : GH_Component
  {
    private int _patternInput;
    private List<string> _patterns = new List<string> { "*.*" };

    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public SaveAsTrackerComponent()
      : base("SaveAsTracker", "Nickname",
          "Copy files from script directory on \"save as\" event",
          "Params", "Util")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      _patternInput = pManager.AddTextParameter("Pattern", "Pattern", "File name pattern", GH_ParamAccess.item, "");
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="da">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess da)
    {
      _patterns = new List<string>{"*.*"};
      var pattern = "";
      if (!da.GetData(_patternInput, ref pattern)) return;
      if (string.IsNullOrWhiteSpace(pattern)) return;
      _patterns = pattern.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
        .ToList();
    }

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// </summary>
    protected override System.Drawing.Bitmap Icon => Resources.Icon;

    public override void AddedToDocument(GH_Document document)
    {
      base.AddedToDocument(document);
      if (document != null)
      {
        document.FilePathChanged += DocumentOnFilePathChanged;
      }
    }

    private void DocumentOnFilePathChanged(object o, GH_DocFilePathEventArgs ghDocFilePathEventArgs)
    {
      ExpireSolution(true);
      if (string.IsNullOrWhiteSpace(ghDocFilePathEventArgs.OldFilePath) ||
          string.IsNullOrWhiteSpace(ghDocFilePathEventArgs.NewFilePath)) return;
      if (ghDocFilePathEventArgs.OldFilePath == ghDocFilePathEventArgs.NewFilePath) return;
      var oldDir = new DirectoryInfo(Path.GetDirectoryName(ghDocFilePathEventArgs.OldFilePath));
      var newDir = new DirectoryInfo(Path.GetDirectoryName(ghDocFilePathEventArgs.NewFilePath));
      var scriptFile = new FileInfo(ghDocFilePathEventArgs.NewFilePath);
      try
      {
        var files = _patterns.SelectMany(x => oldDir.EnumerateFiles(x)).Where(x => x.Name != scriptFile.Name).ToList();
        foreach (var fileInfo in files)
        {
          fileInfo.CopyTo(Path.Combine(newDir.FullName, fileInfo.Name));
        }
      }
      catch (Exception e)
      {
        //
      }
    }

    public override void RemovedFromDocument(GH_Document document)
    {
      base.RemovedFromDocument(document);
      if (document != null) document.FilePathChanged -= DocumentOnFilePathChanged;
    }

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid
    {
      get { return new Guid("08f69654-4427-41c7-8932-0feda25b5949"); }
    }
  }
}
