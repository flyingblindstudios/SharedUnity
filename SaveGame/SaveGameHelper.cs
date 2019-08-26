using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Shared.SaveGame
{
    public class SaveGameHelper
    {
        public enum MODE { BINARY, JSON };
        public static MODE Mode = MODE.BINARY;
        public static int version = 1;


        public static void DeleteSaveGame(string _path)
        {
            File.Delete(_path);
        }

        public async static void WriteSaveGame(ISaveable _saveable, string _path)
        {
            MemoryStream memStream = new MemoryStream();

            AddValue("Dataversion", version, memStream);


            _saveable.WriteTo(memStream, version);

            FileStream fileStream = new FileStream(_path,FileMode.Create, FileAccess.Write);

            memStream.WriteTo(fileStream);

            //memStream.GetBuffer();

            //write buffer to disc async!
            memStream.Flush();
            memStream.Dispose();

            await fileStream.FlushAsync();
            fileStream.Dispose();
                
            //ISaveable

        }

        public static void ReadSaveGameTo(ISaveable _saveable, string _path)
        {
            if(File.Exists(_path))
            { 

                FileStream memStream = new FileStream(_path, FileMode.Open,FileAccess.Read);

                //start async fileread 

                int saveGameVersion = GetInt(memStream);
                _saveable.ReadFrom(memStream, saveGameVersion);

                memStream.Dispose();
            }
        }

        public static void AddValue<T>(string _name,List<T> _list, Stream _stream) where T : new()
        {
            if (!typeof(T).IsSerializable && !(typeof(ISerializable).IsAssignableFrom(typeof(T))))
            {
                throw new InvalidOperationException("A serializable Type is required");
            }

            if (Mode == MODE.BINARY)
            {
                //AddValue("Length", _list.Count, _stream);
                BinaryFormatter formater = new BinaryFormatter();
               /* for (int i = 0; i < _list.Count; i++)
                {
                    formater.Serialize(_stream, _list[i]);
                }*/
                formater.Serialize(_stream, _list);

                /*byte[] bytes = BitConverter.GetBytes(_value.bu);
                 _stream.Write(bytes, 0, bytes.Length);*/
            }
        }

        public static void AddValue( string _name, string _value, Stream _stream )
        {
           /* if (Mode == MODE.BINARY)
            {
                byte[] bytes = BitConverter.GetBytes(_value.bu);
                _stream.Write(bytes, 0, bytes.Length);
            }*/
        }

        public static void AddValue(string _name, float _value, Stream _stream)
        {
            if (Mode == MODE.BINARY)
            {
                byte[] bytes = BitConverter.GetBytes(_value);
                _stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void AddValue(string _name, int _value, Stream _stream)
        {
            // _stream.Write

            if (Mode == MODE.BINARY)
            {
                byte[] bytes = BitConverter.GetBytes(_value);
                _stream.Write(bytes, 0, bytes.Length);
            }

        }

        public static void AddValue(string _name, Vector3 _value, Stream _stream)
        {
            // _stream.Write

            if (Mode == MODE.BINARY)
            {
                AddValue("x", _value.x, _stream);
                AddValue("y", _value.y, _stream);
                AddValue("z", _value.z, _stream);
            }

        }

        public static void AddValue(string _name, Vector2Int _value, Stream _stream)
        {
            // _stream.Write

            if (Mode == MODE.BINARY)
            {
                AddValue("x", _value.x, _stream);
                AddValue("y", _value.y, _stream);
            }

        }

        public static void AddValue(string _name, int[] _value, Stream _stream)
        {
            AddValue(_name+"_size", _value.Length, _stream);

            //_value.

            //for(int i = 0; i < _value.Length; i++)
            byte[] bytes = new byte[_value.Length * 4];

            Buffer.BlockCopy(_value,0, bytes,0, bytes.Length);

            _stream.Write(bytes,0, bytes.Length);

        }

        public static int[] GetInArray(Stream _stream)
        {
            int length = GetInt(_stream);

            byte[] bytes = new byte[length * 4];

            _stream.Read(bytes,0, bytes.Length);

            int[] intArray = new int[length];

            Buffer.BlockCopy(bytes, 0, intArray, 0, bytes.Length);

            return intArray;
        }

        public static Vector3 GetVector3(Stream _stream)
        {
            Vector3 vector = Vector3.zero;
            vector.x = GetFloat(_stream);
            vector.y = GetFloat(_stream);
            vector.z = GetFloat(_stream);
            return vector;
        }

        public static Vector2Int GetVector2Int(Stream _stream)
        {
            Vector2Int vector = Vector2Int.zero;
            vector.x = GetInt(_stream);
            vector.y = GetInt(_stream);
            return vector;
        }

        public static float GetFloat( Stream _stream )
        {
            byte[] bytes = new byte[4];
            _stream.Read(bytes,0, bytes.Length);
            return BitConverter.ToSingle(bytes,0);
        }

        public static int GetInt( Stream _stream )
        {
            byte[] bytes = new byte[4];
            _stream.Read(bytes, 0, bytes.Length);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static string GetString( Stream _stream )
        {
            // _stream.Write
            return "";
        }

        public static List<T> GetList<T>(Stream _stream) where T : new()
        {
            BinaryFormatter formater = new BinaryFormatter();
            /* for (int i = 0; i < _list.Count; i++)
             {
                 formater.Serialize(_stream, _list[i]);
             }*/
            return (List<T>)formater.Deserialize(_stream);
        }
    }
}