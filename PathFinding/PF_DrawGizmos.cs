using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：PF_DrawGizmos  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/6/6 9:46:26
// ================================
namespace Assets.JackCheng.PathFinding
{
    [ExecuteInEditMode]
    public class PF_DrawGizmos : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            //draw row
            for (int i = 0; i <= PF_Util.row; i++)
            {
                Vector3 from = new Vector3(0, 0, i * PF_Util.cellHeight);
                Vector3 to = new Vector3(PF_Util.cellWidth * PF_Util.col, 0, i * PF_Util.cellHeight);
                Gizmos.DrawLine(from, to);
            }

            //draw col
            for (int i = 0; i <= PF_Util.col; i++)
            {
                Vector3 from = new Vector3(i * PF_Util.cellWidth, 0, 0);
                Vector3 to = new Vector3(i * PF_Util.cellWidth, 0, PF_Util.row * PF_Util.cellHeight);
                Gizmos.DrawLine(from, to);
            }

            for (int i = 0; i < PF_Util.listPath.Count; i++)
            {
                if (i + 1 >= PF_Util.listPath.Count)
                    break;
                Vector3 from = PF_Util.listPath[i];
                Vector3 to = PF_Util.listPath[i + 1];
                Gizmos.DrawLine(from, to);
            }
        }
        GameObject goPanel;
        GameObject goCamera;
        void Start()
        {

            goPanel = GameObject.Find("Plane");
            goPanel.transform.localScale = new Vector3(PF_Util.cellWidth * PF_Util.col / 10, 1, PF_Util.cellHeight * PF_Util.row / 10);
            goCamera = GameObject.Find("Main Camera");
        }

        void Update()
        {
            goPanel.transform.position = new Vector3(PF_Util.cellWidth * PF_Util.col / 2.0f, 0, PF_Util.cellHeight * PF_Util.row / 2.0f);
            goCamera.transform.position = new Vector3(PF_Util.cellWidth * PF_Util.col / 2.0f, 200, PF_Util.cellHeight * PF_Util.row / 2.0f);
        }
    }
}
