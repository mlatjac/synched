using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchEd
{
    class SynchedUserAccess
    {
        private string _name;
        private bool _canWrite;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }

        }
        public bool CanWrite
        {
            get
            {
                return _canWrite;
            }
            set
            {
                _canWrite = value;
            }
        }

    }

}
