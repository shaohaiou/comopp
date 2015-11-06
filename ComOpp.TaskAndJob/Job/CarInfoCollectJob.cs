using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComOpp.Components;

namespace ComOpp.TaskAndJob.Job
{
    public class CarInfoCollectJob : IJob
    {
        private static bool isRunning = false;

        public void Execute(System.Xml.XmlNode node)
        {
            if (!isRunning)
            {
                isRunning = true;
                Cars.Instance.CollectData();
                isRunning = false;
            }
        }
    }
}
