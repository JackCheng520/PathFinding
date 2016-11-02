using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.JackCheng.PathFinding
{
    interface IPFQueue<T>
    {
        int Push(T item);

        T Pop();

        T Peek();

        void Modify(int i);
    }
}
