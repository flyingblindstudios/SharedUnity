using System;
using System.IO;
using UnityEngine;

namespace Shared.SaveGame
{
    public class SaveGameHelper
    {
        public enum MODE { BINARY, JSON };
        public static MODE Mode = MODE.BINARY;
        public static int version = 1;

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
    }
}