namespace Sitecore.Support.Shell.Applications.Templates.AddFromTemplate
{
  using System;
  using System.Text.RegularExpressions;
  using System.Web;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.IO;
  using Sitecore.Shell.Applications.Templates.AddFromTemplate;
  using Sitecore.Web;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Sheer;

  public class AddFromTemplateForm: Sitecore.Shell.Applications.Templates.AddFromTemplate.AddFromTemplateForm
  {
    protected override void OnOK(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      this.DoOK();
    }

    /// <summary>
    /// Executes the OK action.
    /// </summary>
    protected new void  DoOK()
    {
      #region Sitecore Support
      this.TemplateName.Value= WebUtil.SafeEncode(this.TemplateName.Value);
      #endregion
      string templateName = this.TemplateName.Value;
      if (String.IsNullOrEmpty(templateName))
      {
        return;
      }

      if (!templateName.StartsWith("/sitecore/templates", StringComparison.InvariantCultureIgnoreCase))
      {
        templateName = FileUtil.MakePath("/sitecore/templates", templateName);
      }

      Item template = Context.ContentDatabase.GetItem(templateName);

      if (template == null)
      {
        SheerResponse.Alert(Translate.Text(
            Texts.THE_TEMPLATE_0_DOES_NOT_EXIST_PLEASE_SELECT_A_TEMPLATE_FROM_THE_LIST_ABOVE,
          this.TemplateName.Value));
        return;
      }

      if (string.IsNullOrEmpty(this.ItemName.Value))
      {
        SheerResponse.Alert(Texts.YOU_MUST_SPECIFY_A_NAME_FOR_THE_NEW_ITEM);
        return;
      }

      if (!Regex.IsMatch(this.ItemName.Value, Settings.ItemNameValidation, RegexOptions.ECMAScript))
      {
        SheerResponse.Alert(Texts.THE_NAME_CONTAINS_INVALID_CHARACTERS);
        return;
      }

      if (this.ItemName.Value.Length > Settings.MaxItemNameLength)
      {
        SheerResponse.Alert(Texts.THE_NAME_IS_TOO_LONG);
        return;
      }

      Registry.SetString("/Current_User/History/Add.From.Template", template.ID.ToString());
      string result = template.ID + "," + this.ItemName.Value;
      Context.ClientPage.ClientResponse.SetDialogValue(result);
      Context.ClientPage.ClientResponse.CloseWindow();
    }
  }
}