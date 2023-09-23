using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWorkflow
{
    private List<ICommand> _commandLst = new List<ICommand>();
    private void Clear()
    {
        _commandLst.Clear();
    }
    public void AddCommand(ICommand command)
    {
        _commandLst.Add(command);
    }

    public BuildWorkflow()
    {

    }

    public void SortCommand()
    {
        _commandLst.Sort((lcommand, rcommand) =>
        {
            return lcommand.Priority.CompareTo(rcommand.Priority);
        }
        );
    }

    public void Excute(CommandArguments args)
    {
        recordLog(args.LogFile, "工作流开始执行");
        try
        {
            foreach(var command in _commandLst)
            {
                recordLog(args.LogFile, "开始指令：" + command.ToString());
                if(!command.Excute(args))
                {
                    recordLog(args.LogFile, command.Error, true);
                    break;
                }
            }
        }
        catch (Exception e)
        {

            recordLog(args.LogFile, "工作流执行异常：" + e.StackTrace);
        }
        recordLog(args.LogFile, "工作流执行结束");
    }
    private void recordLog(string logFile,string log,bool isError=false)
    {
        log = string.Format("{0}{1}:{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), isError ? "[Error]" : "[Info]", log);
        FileUtils.CheckFilePath(logFile);
        FileUtils.WriteFile(logFile, log);
    }
}

public class WorkflowFactroy 
{
    public static BuildWorkflow getBuildSelectResWorkFlow()
    {
        var workflow = new BuildWorkflow();
        workflow.AddCommand(new CollectAssetCommand());
        workflow.AddCommand(new ConvertAssetToABCommand());
        workflow.AddCommand(new ParseAssetRelyCommand());
        workflow.AddCommand(new MakeSelectResCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getBuildBundleWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new CollectAssetCommand());
        workflow.AddCommand(new ConvertAssetToABCommand());
        workflow.AddCommand(new ParseAssetRelyCommand());
        workflow.AddCommand(new MakeAssetBundleCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getBuildBundleByCacheWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new BuildEnvirCommand());
        workflow.AddCommand(new CollectAssetCommand());
        workflow.AddCommand(new ConvertAssetToABCommand());
        workflow.AddCommand(new ParseAssetRelyCommand());
        workflow.AddCommand(new CacheAssetBundleCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getBuildAppWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new CollectAssetCommand());
        workflow.AddCommand(new ConvertAssetToABCommand());
        workflow.AddCommand(new ParseAssetRelyCommand());
        workflow.AddCommand(new MakeAssetBundleCommand());
        workflow.AddCommand(new CompileLuaCommand());
        workflow.AddCommand(new BuildAppCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getOnlyBuildAppWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new BuildAppCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getCompileScriptWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new CompileLuaCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getCopyScriptWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new CopyLuaCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static BuildWorkflow getCopyResWorkflow()
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new CopyResToClientCommand());
        workflow.SortCommand();
        return workflow;
    }

    public static void doWorkflowByCmdLine(CommandArguments args)
    {
        BuildWorkflow workflow = new BuildWorkflow();
        workflow.AddCommand(new BuildEnvirCommand());
        workflow.AddCommand(new CollectAssetCommand());
        workflow.AddCommand(new ConvertAssetToABCommand());
        workflow.AddCommand(new ParseAssetRelyCommand());
        string buildMode = args.getCommandParam(CommandParam.BuildMode, CommandValue.BuildNoneMode);
        if(buildMode.Equals(CommandValue.BuildResMode) || buildMode.Equals(CommandValue.BuildAllMode))
        {
            workflow.AddCommand(new BeforeBuildCommand());
        }
        if(buildMode.Equals(CommandValue.BuildLuaAndAppMode) || buildMode.Equals(CommandValue.BuildResMode) || buildMode.Equals(CommandValue.BuildAllMode))
        {
            workflow.AddCommand(new CacheAssetBundleCommand());
            workflow.AddCommand(new CompileLuaCommand());
        }
        if(buildMode.Equals(CommandValue.BuildAppMode) || buildMode.Equals(CommandValue.BuildLuaAndAppMode) || buildMode.Equals(CommandValue.BuildAllMode))
        {
            workflow.AddCommand(new BuildAppCommand());
        }
        if(args.IsPath)
        {
            workflow.AddCommand(new DiffEnvirCommand());
            workflow.AddCommand(new PrePatchCommand());
            workflow.AddCommand(new BuildPatchCommand());
        }
        workflow.AddCommand(new AfterBuildCommand());
        workflow.SortCommand();
        workflow.Excute(args);
    }
}
