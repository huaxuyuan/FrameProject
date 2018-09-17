using System;
using UnityEditor;
using UnityEngine;

namespace GEngine.Editor
{
    class TableLogic : SingletonNotMono<TableLogic>
    {
        private Action typeChangedCallBack;
        private int _obstacleType;

        public int ObstacleType
        {
            get
            {
                return _obstacleType;
            }
            set
            {
                if (_obstacleType == value) return;

                _obstacleType = value;
                if (typeChangedCallBack != null)
                {
                    typeChangedCallBack();

                }
            }
        }

        private TableData _tableData;
        
        
        public TableData tableData
        {
            get
            { return _tableData; }
        }
        public void InitializeTableLogic()
        {
           
        }
        public void SaveTableLogic()
        {

        }
        public void AddTypeChangeCallBack(Action callback)
        {
            typeChangedCallBack -= callback;
            typeChangedCallBack += callback;
        }
        public void RemoveTypeChangeCallBack(Action callback)
        {
            typeChangedCallBack -= callback;
        }
    }
}
