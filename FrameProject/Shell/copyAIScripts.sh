#!/bin/sh

echo "Copy AI scripts..."
cp -r ../Assets/Scripts/2Logic/GameLogic/AI/* ../../ARMeow/Assets/Scripts/2Logic/GameLogic/AI/

echo "Copy anim scripts..."
cp -r ../Assets/Scripts/4Utility/AnimatorCtrl/* ../../ARMeow/Assets/Scripts/4Utility/AnimatorCtrl/

echo "Copy audio script..."
cp -r ../Assets/Scripts/4Utility/Audio/AudioPlayConfig.cs ../../ARMeow/Assets/Scripts/4Utility/Audio/

echo "Copy behavior action scripts..."
cp -r ../Assets/Scripts/2Logic/GameLogic/BT/* ../../ARMeow/Assets/Scripts/2Logic/GameLogic/BT/

echo "Copy const define..."
cp ../Assets/Scripts/4Utility/constDefineUtility/AIConstDefine.cs ../../ARMeow/Assets/Scripts/4Utility/constDefineUtility/

echo "Copy debug tools..."
cp ../Assets/Scripts/4Utility/debugToolUtility/AIDebugTools.cs ../../ARMeow/Assets/Scripts/4Utility/debugToolUtility/
cp ../Assets/Scripts/4Utility/debugToolUtility/AnimDebugTools.cs ../../ARMeow/Assets/Scripts/4Utility/debugToolUtility/


echo "Copy behavior tree global var"
cp ../Assets/Package/Behavior\ Designer/Resources/BehaviorDesignerGlobalVariables.asset ../../ARMeow/Assets/Package/Behavior\ Designer/Resources/

echo "Copy IK scripts"
cp -r ../Assets/Scripts/4Utility/IKController/* ../../ARMeow/Assets/Scripts/4Utility/IKController/

echo "Done"