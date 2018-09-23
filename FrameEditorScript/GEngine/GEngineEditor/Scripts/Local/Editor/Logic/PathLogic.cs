using System;
using System.Collections.Generic;
using UnityEngine;
using FrameLogicData;
namespace GEngine.Editor
{
    public class PathLogic : SingletonNotMono<PathLogic>
    {
        private GameObject _pathObjParent;
        private VoFrameTotalDetailData _frameTotalDetailData;
        public void InitializePathLogic()
        {
            _pathObjParent = _pathObjParent = GameObject.Find("logic/path_array");
            _frameTotalDetailData = VoConfigDataManager.Instance.currentFrameTotalDetailData;
        }
        public void CreatePathLogic(VoPathData pathData)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<CPC_CameraPath>();
            obj.name = pathData.PathNodeName;
            obj.transform.parent = _pathObjParent.transform;
            _frameTotalDetailData.AddPathData(pathData);
        }
        public void RemovePathLogic(VoPathData pathData)
        {
            Transform pathTrans = _pathObjParent.transform.Find(pathData.PathNodeName);
            if (pathTrans != null)
            {
                GameObject.DestroyImmediate(pathTrans.gameObject);
            }
            _frameTotalDetailData.RemovePathData(pathData);
        }
        public void SavePathLogic()
        {
            List<VoPathData> pathList = _frameTotalDetailData.pathList;
            CPC_CameraPath pathComponent;
            foreach (VoPathData pathData in pathList)
            {
                pathData.ClearPathVariable();
                //get path component 
                pathComponent = _pathObjParent.transform.Find(pathData.PathNodeName).GetComponent<CPC_CameraPath>();
                if (pathComponent.target != null)
                    pathData.PathTarget = GetRendererPath(pathComponent.target.gameObject);
                pathData.LookTarget = pathComponent.lookAtTarget;
                foreach(CPC_Point pathPoint in pathComponent.points)
                {
                    VoPathNode pathNode = pathData.CreatePathNode();
                    pathNode.chained = pathPoint.chained;
                    pathNode.position = pathPoint.position;
                    pathNode.rotation = pathPoint.rotation;
                    pathNode.handlenext = pathPoint.handlenext;
                    pathNode.handleprev = pathPoint.handleprev;
                    pathNode.curveTypePosition = (int)pathPoint.curveTypePosition;
                    pathNode.curveTypeRotation = (int)pathPoint.curveTypeRotation;

                }
                pathData.SavePathNode();
                //get component variable pathData.CreatePathNode(); initialize path node variable

                //pathData.CreatePathNode();
                pathData.SavePathNode();
            }
        }

        public string GetRendererPath(GameObject obj)
        {
            if (obj == null) return "";

            string rendererPath = obj.name;
            Transform parent = obj.transform.parent;
            if (parent == null)
                return rendererPath;
            rendererPath = "/" + parent.name + "/" + rendererPath;
            parent = parent.parent;
            while (parent != null)
            {
                rendererPath = "/" + parent.name + rendererPath;
                parent = parent.parent;
            }
            return rendererPath;
        }
    }
}
