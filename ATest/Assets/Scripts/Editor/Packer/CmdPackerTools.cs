using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using System;

class CmdPackerTools : Editor, IActiveBuildTargetChanged
{
    public int callbackOrder
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        throw new NotImplementedException();
    }

    public static void CmdBuildByTag(BuildTarget tag)
    {
        CommandArguments args = new CommandArguments(System.Environment.GetCommandLineArgs());
        WorkflowFactroy.doWorkflowByCmdLine(args);
    }

    public static void CmdSwitchToBuildTarget(BuildTarget tag)
    {
        if (EditorUserBuildSettings.activeBuildTarget == tag)
        {
            CmdBuildByTag(tag);
        }
        else
        {                                           
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Unknown, tag);
        }
    }

    public static void PublishPCWithParam()
    {
        CmdSwitchToBuildTarget(BuildTarget.StandaloneWindows64);
    }

    public static void PublishAndroidWithParam()
    {
        CmdSwitchToBuildTarget(BuildTarget.Android);
    }

    public static void PublishIosWithParam()
    {
        CmdSwitchToBuildTarget(BuildTarget.iOS);
    }
}
