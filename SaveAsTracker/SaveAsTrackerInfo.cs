using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace SaveAsTracker
{
  public class SaveAsTrackerInfo : GH_AssemblyInfo
  {
    public override string Name
    {
      get
      {
        return "SaveAsTracker";
      }
    }
    public override Bitmap Icon
    {
      get
      {
        //Return a 24x24 pixel bitmap to represent this GHA library.
        return null;
      }
    }
    public override string Description
    {
      get
      {
        //Return a short string describing the purpose of this GHA library.
        return "";
      }
    }
    public override Guid Id
    {
      get
      {
        return new Guid("f78a424d-925f-493d-8dd5-5c7832efd673");
      }
    }

    public override string AuthorName
    {
      get
      {
        //Return a string identifying you or your company.
        return "";
      }
    }
    public override string AuthorContact
    {
      get
      {
        //Return a string representing your preferred contact details.
        return "";
      }
    }
  }
}
