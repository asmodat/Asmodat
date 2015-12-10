using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using System.Drawing;

using System.Reflection;

using System.Data;


using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Asmodat.Abbreviate
{
    public partial class FormsControls
    {



        /// <summary>
        /// This method allows you to get all controls and sub controls from specified control
        /// </summary>
        /// <typeparam name="CntrlType">Type of control object</typeparam>
        /// <param name="CMain">Control that contains controls and subcontrols</param>
        /// <param name="deep">Definess how deep controls schould be searched inside main control</param>
        /// <returns>Returns all controls and subcontrols contained in specified control</returns>
        public static List<CntrlType> GetAllOfType<CntrlType>(Control CMain, int deep = int.MaxValue) where CntrlType : Control
        {
            List<CntrlType> LCTAll = new List<CntrlType>();
            if (CMain.Controls.Count > 0 && deep > 0)

                foreach (Control c in CMain.Controls)
                {
                    if (c == null) continue;
                    else if (c.Controls.Count > 0) LCTAll.AddRange(GetAllOfType<CntrlType>(c,deep - 1));
                    else if (c is CntrlType) LCTAll.Add((CntrlType)c);
                }

            return LCTAll;
        }

        /// <summary>
        /// This method allows you to set p all controls and sub controls specified property from specified control
        /// </summary>
        /// <typeparam name="CntrlType">Type of control object</typeparam>
        /// <param name="CMain">Control that contains controls and subcontrols</param>
        /// <param name="property">Property name that schould be changed</param>
        /// <param name="value">Value of property to change</param>
        /// <param name="invoked">Determines if property changing schould be invoked</param>
        /// <param name="deep">Definess how deep controls schould be searched inside main control (method is used inside thread)</param>
        public static void SetPropertyAllOfType<CntrlType>(Control CMain, string property, object value, bool invoked = false, uint deep = int.MaxValue) where CntrlType : Control
        {
            List<CntrlType> LCTAll = GetAllOfType<CntrlType>(CMain, (int)deep);

            if (CMain.Controls.Count <= 0 || deep <= 0) return;

            foreach (CntrlType CType in LCTAll)
                SetProperty<CntrlType>(CType, property, value, invoked);
        }

        /// <summary>
        /// Sets property of control based by its name
        /// </summary>
        /// <typeparam name="CntrlType">Type of control object</typeparam>
        /// <param name="CType">Control that contains controls and subcontrols</param>
        /// <param name="property">Property name that schould be changed</param>
        /// <param name="value">Value of property to change</param>
        /// <param name="invoked">Determines if property changing schould be invoked (method is used inside thread)</param>
        public static void SetProperty<CntrlType>(CntrlType CType, string property, object value, bool invoked = false) where CntrlType : Control
        {
            if (invoked)
            {
                CType.Invoke((MethodInvoker)(() =>
                {
                    PropertyInfo PInfo = typeof(CntrlType).GetProperty(property);
                    PInfo.SetValue(CType, value, null);
                }));
            }
            else
            {
                PropertyInfo PInfo = typeof(CntrlType).GetProperty(property);
                PInfo.SetValue(CType, value, null);
            }
        }

        /// <summary>
        /// Gets property of control based by its name
        /// </summary>
        /// <typeparam name="CntrlType">Type of control object</typeparam>
        /// <typeparam name="PropertyType">Type of returned property</typeparam>
        /// <param name="CType">Control that contains controls and subcontrols</param>
        /// <param name="property">Property name that schould be returned</param>
        /// <param name="invoked">Determines if property getting schould be invoked (method is used inside thread)</param>
        /// <returns>Returns property of control.</returns>
        public static PropertyType GetProperty<CntrlType, PropertyType>(CntrlType CType, string property, bool invoked = false) where CntrlType : Control
        {
            object value = null;

            if (invoked)
                CType.Invoke((MethodInvoker)(() =>
                {
                    PropertyInfo PInfo = typeof(CntrlType).GetProperty(property);
                    if (PInfo != null) value = PInfo.GetValue(CType, null);
                }));
            else
            {
                PropertyInfo PInfo = typeof(CntrlType).GetProperty(property);
                if (PInfo != null) value = PInfo.GetValue(CType, null);
            }

            return (PropertyType)value;
        }


        //public static void RegistrySave<CntrlType>(Registry reg, Control CMain, string[] parameters, string id, uint deep = int.MaxValue) where CntrlType : Control
        //{
        //    List<CntrlType> LCTAll = GetAllOfType<CntrlType>(CMain, (int)deep);

        //    foreach (CntrlType CType in LCTAll)
        //    {



        //    }

        //}


        public static DataTable ToDataTable(DataGridView DGView)
        {
            DataTable DTable = new DataTable();

            foreach(DataGridViewColumn DGVCollumn in DGView.Columns)
            {
                if (DGVCollumn.Visible)
                    DTable.Columns.Add(DGVCollumn.HeaderText);
            }

            object[] OArray = new object[DGView.Columns.Count];

            foreach (DataGridViewRow DGVRow in DGView.Rows)
            {
                for (int i = 0; i < DGVRow.Cells.Count; i++)
                    OArray[i] = DGVRow.Cells[i].Value;


                DTable.Rows.Add(OArray);
            }


            return DTable;
        }
        public static void UpdateDataGridView(ref DataGridView DGView, DataTable DTable)
        {
            if (DGView.Columns.Count != DTable.Columns.Count)
                return;

            while(DGView.Rows.Count < DTable.Rows.Count)
                   DGView.Rows.Add();

            for (int i = 0; i < DTable.Rows.Count; i++)
            {

                for (int i2 = 0; i2 < DTable.Columns.Count; i2++)
                {
                    string sValue = DTable.Rows[i].ItemArray[i2].ToString();

                    if(DGView[i2, i] is DataGridViewCheckBoxCell)
                    {
                        if (sValue == null || sValue == "") sValue = "False";

                        DGView[i2, i].Value = sValue;
                    }
                    else if(DGView[i2, i] is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxColumn DGVCBColumn = (DataGridViewComboBoxColumn)DGView.Columns[i2];

                        if (!DGVCBColumn.Items.Contains(sValue))
                            DGVCBColumn.Items.Add(sValue);

                        DGView[i2, i].Value = sValue;
                    }
                    else
                    {
                        if (sValue == null || sValue == "") 
                            continue;

                        DGView[i2, i].Value = sValue;
                    }
                }
            }
        }



        public static Dictionary<object, object> ToDataDictionary<CntrlType>(Control CMain, string[] parapeters, int deep = int.MaxValue, bool invoked = false) where CntrlType : Control
        {
            Dictionary<object, object> DO2Data = new Dictionary<object, object>();

           

            if (CMain.Controls.Count > 0 && deep > 0)

                foreach (Control c in CMain.Controls)
                {
                    Dictionary<object, object> DO2SubData = new Dictionary<object, object>();


                    if (c == null) continue;
                    else if (c.Controls.Count > 0) Merge.Dictionary<object, object>(ToDataDictionary<CntrlType>(c, parapeters, deep - 1), ref DO2SubData, invoked);
                    else if (c is CntrlType)
                        foreach (string parameter in parapeters)
                        {
                            object oProperty = GetProperty<CntrlType, object>((CntrlType)c, parameter);
                            if (oProperty == default(object)) continue;
                            DO2SubData.Add(parameter, oProperty);
                        }


                    DO2Data.Add(c.Name, DO2SubData);
                }

            return DO2Data;
        }
        public static void SetFromDataDictionary<CntrlType>(Control CMain, Dictionary<object, object> DO2Data, int deep = int.MaxValue, bool invoked = false) where CntrlType : Control
        {
           
            if (CMain.Controls.Count > 0 && deep > 0)

                foreach(Control c in CMain.Controls)
                {
                    if (c == null) continue;

                    Dictionary<object, object> DO2SubData = null;
                    foreach(object o in DO2Data.Keys)
                    {
                        if (o.ToString() == c.Name)
                            DO2SubData = (Dictionary<object, object>)DO2Data[o];
                    }

                    if (DO2SubData == null) continue;
                    else if (c.Controls.Count > 0) SetFromDataDictionary<CntrlType>(c, DO2SubData, deep - 1);
                    else if (c is CntrlType)
                        foreach (var v in DO2SubData)
                            SetProperty<CntrlType>((CntrlType)c, v.Key.ToString(), v.Value, invoked);
                }
        }

        
        public static void Invoke(Action Action)
        {
            Action.Invoke();
        }

        public static void Invoke( Control invoker, Action Action)
        {
            invoker.Invoke((MethodInvoker)(() =>
            {
                Action.Invoke();
            }));
        }

        /// <summary>
        /// This method invokes passed expression using reference to its parent
        /// Usage example: if(FormsControls.Invoke(() => SomeControl.Items.Count) == 0) ...
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static TResult Invoke<TResult>(Expression<Func<TResult>> expression)
        {
            return expression.Compile().Invoke();
        }

        public static TResult Invoke<TResult, TCntrl>(TCntrl invoker, Expression<Func<TResult>> expression) where TCntrl : Control
        {
            TResult value = default(TResult);
            
                invoker.Invoke((MethodInvoker)(() =>
                {
                    value = expression.Compile().Invoke();
                }));
            
            return value;
        }







        //public static void SetPropertyDGV<CntrlType>(CntrlType CType, string property, object value, bool invoked) where CntrlType : DataGridViewCell
        //{
        //    if (invoked)
        //        CType.Invoke((MethodInvoker)(() =>
        //        {
        //            PropertyInfo PInfo = typeof(CntrlType).GetProperty(property);
        //            PInfo.SetValue(CType, value, null);
        //        }));
        //    else
        //    {
        //        PropertyInfo PInfo = typeof(CntrlType).GetProperty(property);
        //        PInfo.SetValue(CType, value, null);
        //    }
        //}


    }
}

/*
public static TResult Invoke<TResult>(Expression<Func<TResult>> expression)
        {
            //TResult value = default(TResult);
            //Control reference = null;

            //try
            //{
            //    reference = Expressions.GetReference<TResult, Control>(expression);
            //}
            //finally { }


            //reference.Invoke((MethodInvoker)(() =>
            //{
            //    value = expression.Compile()();
            //}));

            return expression.Compile().Invoke();

            //return value;
        }
*/