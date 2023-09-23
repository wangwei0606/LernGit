UIOpenEffectsType={}
UIOpenEffectsType.none="none"
UIOpenEffectsType.zoom="zoom"
UIOpenEffectsType.fallOff="fallOff"
UIOpenEffectsType.fadeOut="fadeOut"
UIOpenEffectsType.rightShow="rightShow"
UIOpenEffectsType.leftShow="leftShow"

OpenMode={}
OpenMode.none="none"
OpenMode.append="append"
OpenMode.ignore="ignore"

CloseMode={}
CloseMode.none="none"
CloseMode.discard="discard"

UIBgType={}
UIBgType.none="none"
UIBgType.WindowBg="WindowBg"
UIBgType.ModalBg="ModalBg"

UIRootNode={}
UIRootNode.LeftTop="LeftTop"
UIRootNode.LeftBottom="LeftBottom"
UIRootNode.RightTop="RightTop"
UIRootNode.RightBottom="RightBottom"
UIRootNode.CenterBottom="CenterBottom"

UIRootNodeHidePos={}
UIRootNodeHidePos[UIRootNode.LeftTop]={-800,0,0}
UIRootNodeHidePos[UIRootNode.LeftBottom]={-800,-250,0}
UIRootNodeHidePos[UIRootNode.RightTop]={800,0,0}
UIRootNodeHidePos[UIRootNode.RightBottom]={800,-250,0}
UIRootNodeHidePos[UIRootNode.CenterBottom]={0,-250,0}

UITag={}
UITag.CompleteEvent="openUICompleteEvent"

RootType={}
RootType.BloodRoot="BloodRoot"
RootType.UIRoot="UIRoot"
RootType.UIStory="UIStory"
RootType.UIMain="UIMain"
RootType.UIPop="UIPop"
RootType.UIPrompt="UIPrompt"
RootType.UIAlert="UIAlert"
RootType.UIGuide="UIGuide"
RootType.UITip="UITip"
RootType.UIMessage="UIMessage"
RootType.UIEffect="UIEffect"

RootDepth={}
RootDepth[RootType.BloodRoot]={depth=100,isAddCanvas=false}
RootDepth[RootType.UIRoot]={depth=200,isAddCanvas=false}
RootDepth[RootType.UIStory]={depth=300,isAddCanvas=true}
RootDepth[RootType.UIMain]={depth=400,isAddCanvas=true}
RootDepth[RootType.UIPop]={depth=500,isAddCanvas=true}
RootDepth[RootType.UIPrompt]={depth=600,isAddCanvas=true}
RootDepth[RootType.UITip]={depth=700,isAddCanvas=true}
RootDepth[RootType.UIAlert]={depth=800,isAddCanvas=true}
RootDepth[RootType.UIGuide]={depth=900,isAddCanvas=true}
RootDepth[RootType.UIMessage]={depth=1000,isAddCanvas=true}