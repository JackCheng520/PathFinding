using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ================================
//* 功能描述：PFQueue  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/6/7 9:18:49
// ================================
namespace Assets.JackCheng.PathFinding
{
    /// <summary>
    /// 最小二叉堆队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PFQueue<T> : IPFQueue<T>
    {
        protected List<T> mlist;
        protected IComparer<T> mComparer;

        public PFQueue()
        {
            mlist = new List<T>();
            mComparer = Comparer<T>.Default;
        }

        public PFQueue(IComparer<T> comparer)
        {
            mlist = new List<T>();
            this.mComparer = comparer;
        }

        public PFQueue(IComparer<T> comparer, int capacity)
        {
            mlist = new List<T>();
            mlist.Capacity = capacity;
            this.mComparer = comparer;
        }

        public int Count
        {
            get
            {
                return mlist.Count;
            }
        }

        /// <summary>
        /// child = (left)parent * 2 +1  ,  (right)parent * 2+2
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Push(T item)
        {
            int mChild = mlist.Count;//4
            mlist.Add(item);
            int mParent;
            do
            {
                if (mChild == 0)
                    break;
                mParent = (mChild - 1) / 2;//2
                if (OnComparer(mParent, mChild) > 0)
                {
                    Swap(mParent, mChild);
                    mChild = mParent;
                }
                else
                    break;
            } while (true);
            return mChild;
        }
        /// <summary>
        /// 提取顶部元素并删除
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T result = mlist[0];
            mlist[0] = mlist[mlist.Count - 1];
            mlist.RemoveAt(mlist.Count - 1);
            int mParent;
            int mleftChild;
            int mRightChild;
            int temp = 0;
            do
            {
                mParent = temp;
                mleftChild = 2 * temp + 1;
                mRightChild = 2 * temp + 2;
                if (Count > mleftChild && OnComparer(temp, mleftChild) > 0)
                {
                    temp = mleftChild;
                }

                if (Count > mRightChild && OnComparer(temp, mRightChild) > 0)
                {
                    temp = mRightChild;
                }
                if (temp == mParent)
                    break;
                Swap(mParent, temp);


            } while (true);

            return result;
        }

        /// <summary>
        /// 提取顶部元素不删除
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (mlist.Count > 0)
                return mlist[0];
            return default(T);
        }
        /// <summary>
        /// 在第i的位置元素变更，从新调整
        /// </summary>
        /// <param name="i"></param>
        public void Modify(int i)
        {
            int idx = i;
            //上滤
            int mParent;
            do
            {
                if (idx == 0)
                    break;
                mParent = (idx - 1) / 2;
                if (OnComparer(mParent, idx) > 0)
                {
                    Swap(mParent, idx);
                    idx = mParent;
                }
                else
                    break;

            } while (true);

            //下滤
            if (idx < i)
                return;
            int mLeftChild;
            int mRightChild;

            do
            {
                mParent = idx;
                mLeftChild = 2 * mParent + 1;
                mRightChild = 2 * mParent + 2;
                if (Count > mLeftChild && OnComparer(idx, mLeftChild) > 0)
                    idx = mLeftChild;
                if (Count > mRightChild && OnComparer(idx, mRightChild) > 0)
                    idx = mRightChild;

                if (idx == mParent)
                    return;
                Swap(mParent, idx);
            } while (true);

        }

        /// <summary>
        /// 交换两个元素的位置
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Swap(int i, int j)
        {
            T temp = mlist[i];
            mlist[i] = mlist[j];
            mlist[j] = temp;
        }
        /// <summary>
        /// 比较两个元素的大小
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int OnComparer(int i, int j)
        {
            return this.mComparer.Compare(mlist[i], mlist[j]);
        }

        public void Clear()
        {
            mlist.Clear();
        }

        public T this[int index]
        {
            set
            {
                mlist[index] = value;
                Modify(index);
            }
            get
            {
                return mlist[index];
            }
        }

    }
}
