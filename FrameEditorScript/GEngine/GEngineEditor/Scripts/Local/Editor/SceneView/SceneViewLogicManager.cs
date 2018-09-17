using System;
using System.Collections.Generic;
using UnityEngine;
using FrameLogicData;
class SceneViewLogicManager : SingletonNotMono<SceneViewLogicManager>
{
    private Dictionary<int,Dictionary<int,SceneViewNode>> _sceneViewNodeDic;
    private GameObject _obstaclePrefab;
    private int _currentRowNum;
    private int _currentColNum;
    public void InitializeSceneView(VoFrameTotalDetailData frameDetailData)
    {
        _sceneViewNodeDic = new Dictionary<int, Dictionary<int, SceneViewNode>>();
        Dictionary<int, SceneViewNode> nodeDic;
        //for (int rowIndex = 0; rowIndex < ConfigDataManager.Instance.ROW_NUM;++rowIndex)
        //{
        //    nodeDic = new Dictionary<int, SceneViewNode>();
        //    _sceneViewNodeDic.Add(rowIndex, nodeDic);
        //    for (int colIndex = 0; colIndex < ConfigDataManager.Instance.COL_NUM; colIndex++)
        //    {

        //        SceneViewNode sceneNode = new SceneViewNode();
        //        sceneNode.InitializeSceneViewNode(_obstaclePrefab,frameTableData.frameObstacleDataDic[rowIndex][colIndex]);
        //        _sceneViewNodeDic[rowIndex][colIndex] = sceneNode;
        //    }
        //}
        //_currentRowNum = ConfigDataManager.Instance.ROW_NUM;
        //_currentColNum = ConfigDataManager.Instance.COL_NUM;
        //DrawObstacle();

    }
    void DrawSceneView()
    {

    }

    /// <summary>
    /// modify obstacle logic
    /// </summary>
    /// <param name="obstacleData"></param>
    //public void ModifyObstacleNodeRender(VoFrameObstacleData obstacleData)
    //{
    //    _sceneViewNodeDic[obstacleData.row][obstacleData.col].ModifyObstacleSceneNode(obstacleData);
    //}
}

