#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace UnityEditor
{
    [InitializeOnLoad]
    public static class LoggerSymbolSetup
    {
        private const string Symbol = "ENABLE_GAME_LOG";

        static LoggerSymbolSetup()
        {
            EnsureSymbol(true);
        }

        internal static void EnsureSymbol(bool add)
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (target == BuildTargetGroup.Unknown) return;

            var symbols = GetSymbols(target);
            bool has = symbols.Contains(Symbol);

            if (add && !has)
            {
                symbols.Add(Symbol);
                SetSymbols(target, symbols);
            }
            else if (!add & has)
            {
                symbols.Remove(Symbol);
                SetSymbols(target, symbols);
            }
        }
        private static List<string> GetSymbols(BuildTargetGroup target)
        {
            NamedBuildTarget named = NamedBuildTarget.FromBuildTargetGroup(target);
            PlayerSettings.GetScriptingDefineSymbols(named, out string[] arr);
            return arr.ToList();
        }

        private static void SetSymbols(BuildTargetGroup target, List<string> symbols)
        {
            NamedBuildTarget named = NamedBuildTarget.FromBuildTargetGroup(target);
            PlayerSettings.SetScriptingDefineSymbols(named, symbols.ToArray());
        }
    }

    public class LoggerBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private const string Symbol = "ENABLE_GAME_LOG";
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            bool isDev = (report.summary.options & BuildOptions.Development) != 0;
            var target = report.summary.platformGroup;

            NamedBuildTarget named = NamedBuildTarget.FromBuildTargetGroup(target);
            PlayerSettings.GetScriptingDefineSymbols(named, out string[] arr);
            var symbols = arr.ToList();

            if (isDev)
            {
                if (!symbols.Contains(Symbol)) symbols.Add(Symbol);
            }
            else
            {
                symbols.Remove(Symbol);
            }

            PlayerSettings.SetScriptingDefineSymbols(named, symbols.ToArray());
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            LoggerSymbolSetup.EnsureSymbol(add: true);
        }
    }
}
#endif