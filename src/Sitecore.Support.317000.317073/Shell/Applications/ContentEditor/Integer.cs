using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Support.Shell.Applications.ContentEditor
{
  public class Integer: Sitecore.Shell.Applications.ContentEditor.Integer
  {
    protected override void DoChange(Message message)
    {
      Value = WebUtil.SafeEncode(Value);
      base.DoChange(message);
    }

  }
}