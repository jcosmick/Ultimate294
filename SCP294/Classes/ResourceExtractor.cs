using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SCP294.Classes
{
    public class ResourceExtractor
    {
        private static string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"EXILED", "Configs", "SCP294");

        public static void extractAudioResources()
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(Properties.Resources));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;
                if (IsOggFile(resource))
                {
                    writeOggResource(resourceKey, (byte[])resource);
                }
            }

        }

        private static void writeOggResource(String name, byte[] res)
        {
            File.WriteAllBytes(Path.Combine(PATH,name+".ogg"), res);
        }

        private static bool IsOggFile(object resource)
        {
            if (resource is byte[])
            {
                byte[] byteArray = (byte[])resource;
                // Check if the byte array is at least 4 bytes long
                if (byteArray.Length < 4)
                    return false;

                // Check if the first 4 bytes match the OGG signature
                return byteArray[0] == 0x4F && byteArray[1] == 0x67 && byteArray[2] == 0x67 && byteArray[3] == 0x53;
            }
            return false;
        }
    }
}
