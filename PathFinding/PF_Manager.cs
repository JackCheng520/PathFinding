using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：PF_Manager  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/6/6 10:26:30
// ================================
namespace Assets.JackCheng.PathFinding
{
    public class PF_Manager : MonoBehaviour
    {
        public Camera mainCamera;

        public Transform sceneGrid;

        private PF_Grid grid;

        private PF_Node beginNode;

        private PF_Node endNode;

        private PFQueue<PF_Node> openList;

        private List<PF_Node> closeList;

        private List<PF_Node> pathList;

        private List<GameObject> listPoint;

        private GameObject goBegin;

        private GameObject goEnd;

        void Awake()
        {
            grid = new PF_Grid();

            openList = new PFQueue<PF_Node>(new PFComparer());

            closeList = new List<PF_Node>();

            pathList = new List<PF_Node>();

            listPoint = new List<GameObject>();
        }


        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name.Equals("Plane"))
                    {
                        int scaleX = PF_Util.cellWidth * PF_Util.col / 10;
                        int scaleY = PF_Util.cellHeight * PF_Util.row / 10;
                        Vector3 pointItem = this.sceneGrid.InverseTransformPoint(hit.point);
                        //Debug.Log(pointItem);
                        pointItem.x = PF_Util.col * 0.5f + Mathf.Ceil(pointItem.x) - 1f;
                        pointItem.z = PF_Util.row * 0.5f + Mathf.Ceil(pointItem.z) - 1f;
                        Debug.Log(" x -- " + (int)pointItem.x + " y -- " + (int)pointItem.z);
                        this.FindPath((int)pointItem.x, (int)pointItem.z);
                    }
                }
            }
        }


        private void FindPath(int _i, int _j)
        {

            PF_Node n = grid.GetNode(_i + "_" + _j);
            if (n == null)
            {
                Debug.LogError("Node can not find" + _i + "_" + _j);
                return;
            }
            if (n.isBarrier)
            {
                Debug.Log("Node is Barrier" + _i + "_" + _j);
                return;
            }
            if (beginNode == null)
            {
                beginNode = n;
                if (goBegin == null)
                {
                    goBegin = PF_Util.LoadObj("target");
                }
                goBegin.transform.position = beginNode.pos;
                goBegin.name = "begin";
                return;
            }
            endNode = n;
            if (goEnd == null)
            {
                goEnd = PF_Util.LoadObj("target");
            }
            goEnd.transform.position = endNode.pos;
            goEnd.name = "end";

            if (openList.Count > 0)
                openList.Clear();
            if (closeList.Count > 0)
                closeList.Clear();
            if (pathList.Count > 0)
                pathList.Clear();


            openList.Push(beginNode);

            ClacWay();

            FindOver();

        }

        private void ClacWay()
        {
            PF_Node parentNode = null;
            while (openList.Count > 0)
            {
                parentNode = openList.Pop();


                //Debug.Log("parentNode -- " + parentNode.id);
                if (parentNode.id.Equals(endNode.id))
                {//找到了
                    PF_Node pNode = closeList[closeList.Count - 1];

                    while (!pNode.id.Equals(beginNode.id))
                    {
                        pathList.Add(pNode);
                        pNode = pNode.parent;
                    }
                    break;
                }
                else
                {
                    bool isContant = false;
                    for (int i = 0, len = closeList.Count; i < len; i++)
                    {
                        if (closeList[i].id.Equals(parentNode.id))
                        {
                            isContant = true;
                            break;
                        }
                    }
                    if (!isContant)
                    {
                        CheckNode(parentNode, endNode);
                    }
                    else
                    {

                    }

                    closeList.Add(parentNode);

                }
            }
        }

        private void FindOver()
        {
            PF_Util.listPath.Clear();
            for (int i = 0; i < listPoint.Count; i++)
            {
                GameObject o = listPoint[i];
                Destroy(o);
                o = null;
            }
            listPoint.Clear();
            pathList.Add(beginNode);
            pathList.Insert(0, endNode);
            for (int i = 0; i < pathList.Count; i++)
            {
                //PF_Util.listPath.Add(pathList[i].pos);
                //Debug.Log(" -- result -- " + pathList[i].id);
                GameObject p = PF_Util.LoadObj("point");
                p.transform.position = pathList[i].pos;
                p.transform.localScale = Vector3.one * 3;
                listPoint.Add(p);
            }
            
            beginNode = endNode;
            goBegin.transform.position = beginNode.pos;
            goEnd.transform.position = new Vector3(10000, 0, 0);
            endNode = null;
        }

        /// <summary>
        /// 四个方向
        /// </summary>
        /// <param name="_parent"></param>
        /// <param name="_endNode"></param>
        private void CheckNode(PF_Node _parent, PF_Node _endNode)
        {
            string[] args = _parent.id.Split('_');
            int i = int.Parse(args[0]);
            int j = int.Parse(args[1]);

            //left
            Vector2 left = new Vector2(i - 1, j);
            string id = left.x + "_" + left.y;
            if (!CheckOutBord(left))
            {
                PF_Node lNode = grid.GetNode(id);
                lNode.g = PF_Util.cost;
                lNode.h = Mathf.Abs(_endNode.i - lNode.i) * PF_Util.cost + Mathf.Abs(_endNode.j - lNode.j) * PF_Util.cost;
                if (!lNode.isBarrier &&
                !IsCloseListContant(id) &&
                !IsOpenListContant(id))
                {
                    openList.Push(lNode);
                    lNode.parent = _parent;
                }
            }
            //right
            Vector2 right = new Vector2(i + 1, j);
            id = right.x + "_" + right.y;
            if (!CheckOutBord(right))
            {
                PF_Node rNode = grid.GetNode(id);
                rNode.g = PF_Util.cost;
                rNode.h = Mathf.Abs(_endNode.i - rNode.i) * PF_Util.cost + Mathf.Abs(_endNode.j - rNode.j) * PF_Util.cost;
                if (!rNode.isBarrier &&
                !IsCloseListContant(id) &&
                !IsOpenListContant(id))
                {
                    openList.Push(rNode);
                    rNode.parent = _parent;
                }
            }
            //up
            Vector2 up = new Vector2(i, j + 1);
            id = up.x + "_" + up.y;
            if (!CheckOutBord(up))
            {
                PF_Node uNode = grid.GetNode(id);
                uNode.g = PF_Util.cost;
                uNode.h = Mathf.Abs(_endNode.i - uNode.i) * PF_Util.cost + Mathf.Abs(_endNode.j - uNode.j) * PF_Util.cost;
                if (!uNode.isBarrier &&
                !IsCloseListContant(id) &&
                !IsOpenListContant(id))
                {
                    openList.Push(uNode);
                    uNode.parent = _parent;
                }
            }
            //down 
            Vector2 down = new Vector2(i, j - 1);
            id = down.x + "_" + down.y;
            if (!CheckOutBord(down))
            {
                PF_Node dNode = grid.GetNode(id);
                dNode.g = PF_Util.cost;
                dNode.h = Mathf.Abs(_endNode.i - dNode.i) * PF_Util.cost + Mathf.Abs(_endNode.j - dNode.j) * PF_Util.cost;
                if (!dNode.isBarrier &&
                !IsCloseListContant(id) &&
                !IsOpenListContant(id))
                {
                    openList.Push(dNode);
                    dNode.parent = _parent;
                }
            }


        }

        private bool CheckOutBord(Vector2 v)
        {
            bool flag = false;
            if (v.x < 0)
                flag = true;
            if (v.x >= PF_Util.col)
                flag = true;
            if (v.y < 0)
                flag = true;
            if (v.y >= PF_Util.row)
                flag = true;

            return flag;
        }

        private bool IsCloseListContant(string id)
        {
            for (int i = 0, len = closeList.Count; i < len; i++)
            {
                if (closeList[i].id.Equals(id))
                    return true;
            }
            return false;
        }

        private bool IsOpenListContant(string id)
        {
            for (int i = 0, len = openList.Count; i < len; i++)
            {
                if (openList[i].id.Equals(id))
                    return true;
            }
            return false;
        }


    }

    public class PFComparer : IComparer<PF_Node>
    {
        public int Compare(PF_Node a, PF_Node b)
        {
            if (a.f > b.f)
                return 1;
            if (a.f < b.f)
                return -1;
            return 0;
        }
    }
}
