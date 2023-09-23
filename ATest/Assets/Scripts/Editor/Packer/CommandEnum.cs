using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandEnum
{
    // 打包工作流程优先顺序  
    BuildEnvir,     //构建打包环境
    BuildBefore,   //打包之前                
    CollectAsset,    //收集资源
    ConvertAssetToAB,//转化资源为AB
    ParseAssetRely,  //解析资源依赖
    DiffEnvir,        //构建补丁环境
    CacheAssetBundle, //缓存打包AB
    MakeAssetBundle,  //生成AB
    MakeSelectRes,    //编译选择的AB
    CompileRes,       //处理资源
    CompileLua,      //处理lua脚本
    PrePatch,         //准备补丁包资源
    BuildPatch,      //只做补丁包
    ZipRes,          //压缩资源
    BuildApp,        //生成APP
    SaveSetting,     //保存配置
    BuildAfter,      //构建完成后
    ClearEnvir,      //清理环境
}
