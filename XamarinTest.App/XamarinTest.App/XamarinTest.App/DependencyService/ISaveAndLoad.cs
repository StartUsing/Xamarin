using System.Reflection;
using System.Threading.Tasks;

namespace XamarinTest.App
{
    public interface ISaveAndLoad
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
        void RemoveText(string filename);
        string SaveCache(string filename, byte[] bytes);
        byte[] Load(string filename);
    }
}