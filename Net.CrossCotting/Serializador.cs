using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Net.CrossCotting
{
    public class Serializador
    {
        #region xml
        public bool SerializarXml<T>(T entidad, Stream stm)
        {
            bool result = false;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(stm, entidad);
                result = true;
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public T DeserializarXml<T>(Stream entidad) where T : class
        {
            T result = null;
            entidad.Position = 0;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            result = ser.Deserialize(entidad) as T;
            return result;
        }

        #endregion
        #region binario
        public bool SerializarBinario<T>(T entidad, Stream stm)
        {
            bool result = false;
            try
            {
                BinaryFormatter serilializador = new BinaryFormatter();
                serilializador.Serialize(stm, entidad);
                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public T DeserializarBinario<T>(Stream entidad) where T : class
        {
            T Result = null;
            try
            {
                entidad.Position = 0;
                BinaryFormatter serializador = new BinaryFormatter();
                Result = serializador.Deserialize(entidad) as T;

            }
            catch (Exception)
            {

                throw;
            }
            return Result;
        }


        #endregion
        #region json

        public bool SerializarJson<T>(T entidad, Stream stm)
        {
            bool result = false;
            try
            {
                // ReSharper disable once SuggestUseVarKeywordEvident
                DataContractJsonSerializer jentidad = new DataContractJsonSerializer(typeof(T));
                jentidad.WriteObject(stm, entidad);
                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public T DeserializarJson<T>(Stream entidad) where T : class
        {
            T result = null;
            try
            {
                entidad.Position = 0;
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

                result = (T)ser.ReadObject(entidad);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        #endregion
    }
}
