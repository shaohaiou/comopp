using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ComOpp.Components
{
    public interface IJob
    {
        void Execute(XmlNode node);
    }
}
