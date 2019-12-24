using System.Collections.Generic;

namespace BurnoutConsole
{
    public class SerializedCollectionInfo
    {
        public string Name { get; set; }
        public List<SerializedCollectionInfo> Children { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
}