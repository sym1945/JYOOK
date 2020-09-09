using System.IO;
using System.Xml.Serialization;

namespace JYOOK.Infrastructure.Data
{
    public class XmlSerializer<T>
    {
        private readonly object _Locker = new object();

        private readonly string _FileFullName;

        public XmlSerializer(string fileFullName)
        {
            _FileFullName = fileFullName;
        }

        public T LoadXml()
        {
            lock (_Locker)
            {
                using (var streamReader = new StreamReader(_FileFullName))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(streamReader);
                }
            }
        }

        public void SaveXml(T livestockProducts)
        {
            lock (_Locker)
            {
                var fileInfo = new FileInfo(_FileFullName);
                if (fileInfo.Directory.Exists == false)
                    fileInfo.Directory.Create();

                using (var streamWriter = new StreamWriter(_FileFullName))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, livestockProducts);
                }
            }
        }

    }
}