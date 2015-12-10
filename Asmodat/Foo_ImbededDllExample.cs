/*
//Add new "existing item" to project (.dll) as "Imbeded Resource" and "do not copy"
 
 * 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;

using System.IO;
         static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve; // ------------------------------ Dont forget to add assembly resolving !!! <-- LOOK HERE

            Application.Run(new Form1());
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            

            Assembly Assmbly = Assembly.GetExecutingAssembly();
            var vRList = Assmbly.GetManifestResourceNames().ToList();
            string sName = null;

            foreach(string s in vRList)
                if(s.Contains(".dll"))
                {
                    sName = s;
                    break;
                }

            Stream Strm = Assmbly.GetManifestResourceStream(sName);

            using (var vStream = Strm)
            {
                byte[] baData = new byte[vStream.Length];
                vStream.Read(baData, 0, baData.Length);
                return Assembly.Load(baData);
            }
        }
    } 
     
     */



