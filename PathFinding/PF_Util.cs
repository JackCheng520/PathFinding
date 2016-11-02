using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：PF_Util  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/6/6 9:53:27
// ================================
namespace Assets.JackCheng.PathFinding
{
    public class PF_Util
    {
        static public int cellWidth = 20;
        static public int cellHeight = 20;

        static public int row = 10;
        static public int col = 10;

        static public int cost = 20;
        static public int diagonalCost = 28;

        static public Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

        static public GameObject LoadObj(string name)
        {
            GameObject obj = null;
            if (objDic.ContainsKey(name))
                obj = GameObject.Instantiate(objDic[name], Vector3.zero, Quaternion.identity) as GameObject;
            else
            {
                GameObject prefab = Resources.Load<GameObject>(name);
                obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                objDic.Add(name, prefab);
            }
            obj.transform.localScale = new Vector3(PF_Util.cellWidth, 2, PF_Util.cellHeight);

            return obj;
        }

        //list path 
        static public List<Vector3> listPath = new List<Vector3>();
    }
}
