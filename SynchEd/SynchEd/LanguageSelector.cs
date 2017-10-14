using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchEd
{
    public class LanguageSelector
    {
        private String _OriginalLanguageName, _EnglishNameOfLanguage, _IETFCode;

        public String IETFCode
        {
            get
            {
                return _IETFCode;
            }
            set
            {
                _IETFCode = value;
            }
        }
        public String OriginalLanguageName
        {
            get
            {
                return _OriginalLanguageName;
            }
            set
            {
                _OriginalLanguageName = value;
            }
        }
        public String EnglishNameOfLanguage
        {
            get
            {
                return _EnglishNameOfLanguage;
            }
            set
            {
                _EnglishNameOfLanguage = value;
            }
        }
    }
}
