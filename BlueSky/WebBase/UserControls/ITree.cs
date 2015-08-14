using System;
using System.Collections.Generic;
using System.Text;

namespace WebBase.UserControls
{
    public interface ITree<T> where T : class
    {
        List<T> List();
        List<T> List(object _LinkStartValue);
    }
}
