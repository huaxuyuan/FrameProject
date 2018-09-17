#!/bin/bash
SourceROOT=$( cd "$( dirname "$0"  )" && cd ..&& pwd  )
TargetROOT=$( cd "$( dirname "$0"  )" && cd ..&& cd ..&& pwd  )
ConfigName=/Config
projectName=/Assets/StreamingAssets/Android
targetName=/DollHouseScript
sourcePath=${SourceROOT}${projectName}
targetPath=${TargetROOT}${targetName}${projectName}
batPath=${TargetROOT}${ConfigName}
echo "sourcePath : "$sourcePath
echo "targetPath : "$targetPath
echo "ROOT       : "$ROOT

find ${sourcePath} -type f -name *.szpkg | xargs cp --target-directory=${targetPath}