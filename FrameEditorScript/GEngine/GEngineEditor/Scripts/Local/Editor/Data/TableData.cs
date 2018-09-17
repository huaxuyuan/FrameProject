using UnityEditor;
using UnityEngine;

namespace GEngine.Editor
{
    public enum ObstacleType
    {
        Occupy, //地格
        Block,
        Cover   //罩子
    }

    class TableData
    {
        public ObstacleType Type { get; set; }
    }
}
