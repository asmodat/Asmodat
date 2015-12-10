using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>

/// </summary>
namespace Asmodat
{
    public static class Foo
    {
        public static void nop(){}
    }
}
//Instance
/*
private static volatile Manager instance;
        private static object syncRoot = new Object();

        public bool LOADED = false;

        public static Manager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Manager();
                        }
                    }
                }

                return instance;
            }
        }
*/