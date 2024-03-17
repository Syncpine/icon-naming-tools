using System.Collections.Generic;

namespace icon_naming_tools
{
    public class IconInfo
    {
        public string DirectoryPath;

        public string UpdateDirectoryPath;

        public string Extension;

        public readonly List<string> IconPathList = new List<string>();

        public readonly List<string> UpdateIconPathList = new List<string>();
    }
}