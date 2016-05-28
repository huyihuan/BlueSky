using System;
using System.Collections.Generic;
using System.Text;

namespace WebBase.UserControls
{
    public interface ITree<T> where T : class
    {
        ITreeNodeData TreeNodeData { get; }
        List<T> TreeList();
        List<T> TreeList(object _LinkStartValue);
    }
}
