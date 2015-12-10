using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asmodat.Abbreviate
{
    public static class WebControls
    {
        public static List<CntrlType> GetAllOfType<CntrlType>(Control CMain) where CntrlType : Control
        {
            List<CntrlType> LCTAll = new List<CntrlType>();
            if (CMain.Controls.Count > 0)

                foreach (Control c in CMain.Controls)
                {
                    if (c == null) continue;
                    else if (c.Controls.Count > 0) LCTAll.AddRange(GetAllOfType<CntrlType>(c));
                    else if (c is CntrlType) LCTAll.Add((CntrlType)c);
                }

            return LCTAll;
        }

        public static List<Control> GetAllControls(ControlCollection CCollection)
        {
            List<Control> LCTAll = new List<Control>();
            if (CCollection.Count > 0)

                foreach (Control c in CCollection)
                {
                    if (c == null) continue;
                    else if (c.Controls.Count > 0) LCTAll.AddRange(GetAllOfType<Control>(c));
                    else if (c is Control) LCTAll.Add((Control)c);
                }

            return LCTAll;
        }

        public static List<RetType> GetAllControls<RetType>(ControlCollection CCollection) where RetType : Control
        {
            List<RetType> LRTAll = new List<RetType>();
            if (CCollection.Count > 0)

                foreach (Control c in CCollection)
                {
                    
                    if (c == null) continue;
                    else if (c.Controls.Count > 0) LRTAll.AddRange(GetAllOfType<RetType>(c));
                    else if (c is RetType) LRTAll.Add((RetType)c);
                }

            return LRTAll;
        }

    }
}