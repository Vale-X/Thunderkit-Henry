using System;
using System.Linq;
using System.Reflection;

namespace RoR2EditorKit.Utilities
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Gets all the types from an assembly safely by getting the types from a ReflectionTypeLoadException if one is thrown
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="assemblyTypes"></param>
        /// <returns>true if a ReflectionTypeLoadException was caught</returns>
        public static bool GetTypesSafe(Assembly assembly, out Type[] assemblyTypes)
        {
            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException re)
            {
                assemblyTypes = re.Types.Where(t => t != null).ToArray();
                return true;
            }

            return false;
        }
    }
}