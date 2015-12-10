using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Principal;

using System.Windows.Forms;

namespace Asmodat.Notifications
{
    public static class FormsInfo
    {
        public static bool IsUserRole(WindowsBuiltInRole WBIRole = WindowsBuiltInRole.Administrator, bool bShowExceptionMessage = true)
        {
            bool bIsAdmin = false;

            try
            {
                WindowsIdentity WIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal WPrincipal = new WindowsPrincipal(WIdentity);
                bIsAdmin = WPrincipal.IsInRole(WBIRole);
            }
            catch
            {
                bIsAdmin = false;
            }

            if (!bShowExceptionMessage)
                MessageBox.Show("You do not have sufficient permissions to run this program, it might not work, try to run it as Administartor !");
            

            return bIsAdmin;
        }
    }
}
