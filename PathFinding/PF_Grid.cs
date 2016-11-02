using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：PF_Grid  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/6/6 10:27:07
// ================================
namespace Assets.JackCheng.PathFinding
{
    public class PF_Grid
    {
        public PF_Grid()
        {
            nodeDic = new Dictionary<string, PF_Node>();
            barrierList = new List<GameObject>();
            Init();
        }
        ~PF_Grid()
        {
            nodeDic = null;
            if (barrierList.Count > 0)
            {
                for (int i = 0; i < barrierList.Count; i++)
                {
                    GameObject o = barrierList[i];
                    if (o != null)
                    {
                        GameObject.Destroy(o);
                        o = null;
                    }
                }
            }
            barrierList.Clear();
            barrierList = null;
        }

        public Dictionary<string, PF_Node> nodeDic;

        public List<GameObject> barrierList;

        private void Init()
        {
            for (int i = 0; i < PF_Util.row; i++)
            {
                for (int j = 0; j < PF_Util.col; j++)
                {
                    int randNum = UnityEngine.Random.Range(0, 5);
                    //Debug.Log(randNum);
                    bool isBarrier = randNum > 3 ? true : false;
                    PF_Node n = null;
                    Vector3 pos = new Vector3((j + 0.5f) * PF_Util.cellWidth, 0, (i + 0.5f) * PF_Util.cellHeight);
                    n = new PF_Node(j + "_" + i, isBarrier, pos);
                    AddNode(n);
                }
            }
        }

        private void AddNode(PF_Node node)
        {
            if (nodeDic.ContainsKey(node.id))
                nodeDic[node.id] = node;
            else
                nodeDic.Add(node.id, node);
        }

        public PF_Node GetNode(string _id)
        {
            if (nodeDic.ContainsKey(_id))
            {
                return nodeDic[_id];
            }
            return null;
        }
    }
}
