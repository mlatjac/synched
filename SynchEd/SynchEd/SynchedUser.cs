using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchEd
{
    public class SynchedUser
    {
        private string _name;
        private int _id;
        private string _userKey;

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
        public string UserKey
        {
            get
            {
                return _userKey;
            }
            set
            {
                _userKey = value;
            }
        }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
    }

    public class SynchedDocument
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
