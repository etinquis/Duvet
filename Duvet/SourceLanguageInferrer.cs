using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duvet
{
    class SourceLanguageInferrer
    {
        private static Dictionary<string, SourceLanguage> _languages;
 
        static SourceLanguageInferrer()
        {
            _languages = new Dictionary<string, SourceLanguage>();

            _languages.Add(".cpp", SourceLanguage.Cpp);
            _languages.Add(".h", SourceLanguage.Cpp);
            _languages.Add(".cs", SourceLanguage.CSharp);
        }

        public static SourceLanguage InferFrom(string fileName)
        {
            FileInfo info = new FileInfo(fileName);

            if (_languages.ContainsKey(info.Extension))
            {
                return _languages[info.Extension];
            }

            return SourceLanguage.Undefined;
        }
    }
}
