using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System;

public class PackerTools : Editor,IActiveBuildTargetChanged
{
    public int callbackOrder
    {
        get
        {
            throw new NotImplementedException();
        }
    }
  
    public static void SwitchToBuildTarget(BuildTarget tag)
    {
        if(EditorUserBuildSettings.activeBuildTarget==tag)
        {
            BuildByTag(tag);
        }
        else
        {

            EditorUserBuildSettings.activeBuildTargetChanged = delegate ()
            {
               if (EditorUserBuildSettings.activeBuildTarget == tag)
               {
                   BuildByTag(tag);
               }
            };
            EditorUserBuildSettings.SwitchActiveBuildTarget(tag);
            //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Unknown, tag);
        }
    }
    public static void BuildByTag(BuildTarget tag)
    {
        Debug.LogError("11");
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getBuildAppWorkflow().Excute(args);
    }

    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        if (EditorUserBuildSettings.activeBuildTarget == newTarget)
        {
        //    BuildByTag(newTarget);
        }
    }

    public static void BuildAndroid()
    {
        SwitchToBuildTarget(BuildTarget.Android);
    }

    public static void BuildPC()
    {
        SwitchToBuildTarget(BuildTarget.StandaloneWindows64);
    }

    public static void BuildIOS()
    {
        SwitchToBuildTarget(BuildTarget.iOS);
    }

    public static void OnlyPublish()
    {
        CommandArguments args=new CommandArguments() ;
        WorkflowFactroy.getOnlyBuildAppWorkflow().Excute(args);
    }

    public static void CompileSelectRes()
    {
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getBuildSelectResWorkFlow().Excute(args);
    }

    public static void CompileScripts()
    {
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getCompileScriptWorkflow().Excute(args);
    }

    public static void CopyScripts()
    {
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getCopyScriptWorkflow().Excute(args);
    }

    public static void CopyResToClient()
    {
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getCopyResWorkflow().Excute(args);
    }

    public static void CompileRes()
    {
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getBuildBundleWorkflow().Excute(args);
    }

    public static void TestCacheBuild()
    {
        CommandArguments args = new CommandArguments();
        WorkflowFactroy.getBuildBundleByCacheWorkflow().Excute(args);
    }


}
