using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.Server.Tests.ACLogview_Imports.Message_Deserializers
{
    public class CM_House
    {
        public class RestrictionDB
        {
            // The latest client and server used 0x10000002 for the version variable.
            public uint version;
            public uint _bitmask;
            public uint _monarch_iid;
            public PackableHashTable<uint, uint> _table;
            public int Length;

            public static RestrictionDB read(BinaryReader binaryReader)
            {
                RestrictionDB newObj = new RestrictionDB();
                var startPosition = binaryReader.BaseStream.Position;
                newObj.version = binaryReader.ReadUInt32();
                newObj._bitmask = binaryReader.ReadUInt32();
                newObj._monarch_iid = binaryReader.ReadUInt32();
                newObj._table = PackableHashTable<uint, uint>.read(binaryReader);
                newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
                return newObj;
            }
        }
    }
}
