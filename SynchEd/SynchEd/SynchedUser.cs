using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchEd
{
    class SynchedUser
    {
        private string _name;

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
    }

    class SynchedDocument
    {
        //private SynchedUser _owner;
        private string _ownwer;
        private string _name;

        public string OwnerName
        {
            get
            {
                //return _owner.Name;
                return _ownwer;
            }
            set
            {
                _ownwer = value;
            }
        }
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
    }
}
