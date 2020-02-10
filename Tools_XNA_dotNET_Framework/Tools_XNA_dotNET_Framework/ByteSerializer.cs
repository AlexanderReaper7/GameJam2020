using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Tools_XNA_dotNET_Framework
{
    public static class ByteSerializer
    {
        /// <summary>
        /// Converts an object to a byte array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts a byte array to an Object
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        /// <inheritdoc cref="ByteArrayToObject"/>
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            return (T) ByteArrayToObject(arrBytes);
        }
    }
}
