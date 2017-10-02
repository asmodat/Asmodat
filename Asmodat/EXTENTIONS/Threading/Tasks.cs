using System.Threading.Tasks;

namespace Asmodat.Extensions.Threading
{
    public static class TasksEx
    {
        public static T Await<T>(this Task<T> t)
        {
            t.Wait();
            return t.Result;
        }
    }
}
