using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：PF_Node  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/6/6 10:20:41
// ================================
namespace Assets.JackCheng.PathFinding
{
    public class PF_Node
    {
        public PF_Node(string _id, bool _isBarrier, Vector3 pos)
        {
            this.id = _id;
            string[] args = this.id.Split('_');
            i = int.Parse(args[0]);
            j = int.Parse(args[1]);
            this.isBarrier = _isBarrier;
            if (this.isBarrier)
            {
                GameObject obj = PF_Util.LoadObj("node");
                obj.transform.position = pos;
            }
            this.pos = pos;
        }
        ~PF_Node() { }

        public string id;

        public PF_Node parent;

        public Vector3 pos;

        public int i;

        public int j;

        public int g;

        public int h;

        public int f
        {
            get
            {
                return g + h;
            }
        }

        public bool isBarrier;
    }
}
