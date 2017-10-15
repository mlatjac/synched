using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchEd
{
    public class SynchedDocument
    {
        private String _name;
        private SynchedUser _owner;
        private String _xamlContent;
        private int _id;


        public string OwnerName
        {
            get
            {
                return _owner.Name;
            }
        }
        public SynchedUser Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
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
        public string XamlContent
        {
            get
            {
                return _xamlContent;
            }
            set
            {
                _xamlContent = value;
            }
        }
    }
}


