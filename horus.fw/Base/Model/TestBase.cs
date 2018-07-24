using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static horus.Base.Base;

namespace horus.Base.Model
{
    public class TestBase
    {
        public int Index { get; private set; }

        public string ID { get; private set; }

        public string Name { get; private set; }

        public Status Status { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public TimeSpan Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        public object Value { get; private set; }

        public string Comment { get; private set; }

        public string Error { get; private set; }

        public bool IsStopper { get; private set; }

        public TestBase()
        {
            StartTime = DateTime.Now;
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void SetID(string id)
        {
            ID = id;
        }

        public void SetName(string name)
        {
            Name = name;
        }
        
        public void SetStatus(Status status)
        {
            Status = status;
        }

        public void SetValue(object returnValue)
        {
            Value = returnValue;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }

        public void SetError(string message)
        {
            Error = message;
        }

        public void SetMasterTestCase(bool isStopper)
        {
            IsStopper = isStopper;
        }

        public void Finish()
        {
            EndTime = DateTime.Now;
        }
    }
}
