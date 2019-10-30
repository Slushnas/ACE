using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ACE.Server.Tests.ACLogview_Imports
{
    public class Util
    {
        public static IDictionary<Type, Func<BinaryReader, dynamic>> readers = null;

        public static void initReaders()
        {
            if (readers == null)
            {
                readers = new Dictionary<Type, Func<BinaryReader, dynamic>>();
                readers.Add(typeof(byte), r => r.ReadByte());
                readers.Add(typeof(ushort), r => r.ReadUInt16());
                readers.Add(typeof(short), r => r.ReadInt16());
                readers.Add(typeof(uint), r => r.ReadUInt32());
                readers.Add(typeof(int), r => r.ReadInt32());
                readers.Add(typeof(ulong), r => r.ReadUInt64());
                readers.Add(typeof(long), r => r.ReadInt64());
                readers.Add(typeof(float), r => r.ReadSingle());
                readers.Add(typeof(double), r => r.ReadDouble());

                readers.Add(typeof(PStringChar), r => PStringChar.read(r));

                //readers.Add(typeof(STypeInt), r => (STypeInt)r.ReadUInt32());
                //readers.Add(typeof(STypeInt64), r => (STypeInt64)r.ReadUInt32());
                //readers.Add(typeof(STypeBool), r => (STypeBool)r.ReadUInt32());
                //readers.Add(typeof(STypeFloat), r => (STypeFloat)r.ReadUInt32());
                //readers.Add(typeof(STypeString), r => (STypeString)r.ReadUInt32());
                //readers.Add(typeof(STypeDID), r => (STypeDID)r.ReadUInt32());
                //readers.Add(typeof(STypePosition), r => (STypePosition)r.ReadUInt32());
                //readers.Add(typeof(STypeIID), r => (STypeIID)r.ReadUInt32());
                //readers.Add(typeof(STypeSkill), r => (STypeSkill)r.ReadUInt32());

                //readers.Add(typeof(CharacterTitle), r => (CharacterTitle)r.ReadUInt32());
                //readers.Add(typeof(SKILL_ADVANCEMENT_CLASS), r => (SKILL_ADVANCEMENT_CLASS)r.ReadUInt32());

                //readers.Add(typeof(CM_Magic.Enchantment), r => CM_Magic.Enchantment.read(r));
                //readers.Add(typeof(CM_Magic.EnchantmentID), r => CM_Magic.EnchantmentID.read(r));
                //readers.Add(typeof(CM_Social.FriendData), r => CM_Social.FriendData.read(r));
                //readers.Add(typeof(CM_Social.CContractTracker), r => CM_Social.CContractTracker.read(r));
                //readers.Add(typeof(Attribute), r => Attribute.read(r));
                //readers.Add(typeof(SecondaryAttribute), r => SecondaryAttribute.read(r));
                //readers.Add(typeof(Position), r => Position.read(r));
                //readers.Add(typeof(Skill), r => Skill.read(r));

                //readers.Add(typeof(SpellID), r => (SpellID)r.ReadUInt32());
                //readers.Add(typeof(CM_Vendor.ItemProfile), r => CM_Vendor.ItemProfile.read(r));
                //readers.Add(typeof(CM_Fellowship.Fellow), r => CM_Fellowship.Fellow.read(r));
                //readers.Add(typeof(CM_Fellowship.LockedFellowshipList), r => CM_Fellowship.LockedFellowshipList.read(r));
                //readers.Add(typeof(CM_Inventory.ContentProfile), r => CM_Inventory.ContentProfile.read(r));
                //readers.Add(typeof(CM_Login.InventoryPlacement), r => CM_Login.InventoryPlacement.read(r));
                //readers.Add(typeof(CM_Login.MissingIteration), r => CM_Login.MissingIteration.read(r));
                //readers.Add(typeof(CM_Login.CAllIterationList), r => CM_Login.CAllIterationList.read(r));
                //readers.Add(typeof(CM_Login.PTaggedIterationList), r => CM_Login.PTaggedIterationList.read(r));
                //readers.Add(typeof(CM_Login.CMostlyConsecutiveIntSet), r => CM_Login.CMostlyConsecutiveIntSet.read(r));
                //readers.Add(typeof(CM_House.HousePayment), r => CM_House.HousePayment.read(r));
                //readers.Add(typeof(CM_Inventory.SalvageResult), r => CM_Inventory.SalvageResult.read(r));
                //readers.Add(typeof(CM_Writing.PageData), r => CM_Writing.PageData.read(r));
                //readers.Add(typeof(CM_House.GuestInfo), r => CM_House.GuestInfo.read(r));
            }
        }

        /// <summary>
        /// Calculates and reads any padding bytes needed to align to a dword boundary.
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>Returns a byte with the number of padding bytes read.</returns>
        public static byte readToAlign(BinaryReader binaryReader)
        {
            long alignDelta = binaryReader.BaseStream.Position % 4;
            if (alignDelta != 0)
            {
                binaryReader.ReadBytes((int)(4 - alignDelta));
                return (byte)(4 - alignDelta);
            }
            return 0;
        }

        public static string readUnicodeString(BinaryReader binaryReader)
        {
            uint strLen = binaryReader.ReadByte();
            // If string length is >= 128 a second byte is present and 
            // the least significant bits are used to calculate the length.
            if ((strLen & 0x80) > 0) // PackedByte
            {
                byte lowbyte = binaryReader.ReadByte();
                strLen = ((strLen & 0x7F) << 8) | lowbyte;
            }
            string str = "";
            if (strLen != 0)
            {
                for (uint i = 0; i < strLen; i++)
                {
                    str += Encoding.Unicode.GetString(binaryReader.ReadBytes(2));
                }
            }

            return str;
        }
    }
    public class Packets
    {
        public static uint readDataIDOfKnownType(uint i_didFirstID, BinaryReader binaryReader)
        {
            ushort offset = binaryReader.ReadUInt16();

            if ((offset & 0x8000) == 0)
            {
                return i_didFirstID + offset;
            }
            else
            {
                ushort offsetHigh = binaryReader.ReadUInt16();
                return i_didFirstID + (uint)(offsetHigh | ((offset & 0x3FFF) << 16));
            }
        }

        public static uint readWClassIDCompressed(BinaryReader binaryReader)
        {
            ushort id = binaryReader.ReadUInt16();

            if ((id & 0x8000) == 0)
            {
                return id;
            }
            else
            {
                ushort idHigh = binaryReader.ReadUInt16();
                return (uint)(idHigh | ((id & 0x7FFF) << 16));
            }
        }
    }

    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public static Vector3 read(BinaryReader binaryReader)
        {
            Vector3 newObj = new Vector3();
            newObj.x = binaryReader.ReadSingle();
            newObj.y = binaryReader.ReadSingle();
            newObj.z = binaryReader.ReadSingle();
            return newObj;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(x);
            builder.Append(" ");
            builder.Append(y);
            builder.Append(" ");
            builder.Append(z);

            return builder.ToString();
        }
    }

    public class Position
    {
        public uint objcell_id;
        public Frame frame = new Frame();
        public int Length;

        public static Position read(BinaryReader binaryReader)
        {
            Position newObj = new Position();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.objcell_id = binaryReader.ReadUInt32();
            newObj.frame = Frame.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public static Position readOrigin(BinaryReader binaryReader)
        {
            Position newObj = new Position();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.objcell_id = binaryReader.ReadUInt32();
            newObj.frame.m_fOrigin = Vector3.read(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }
    }

    public class Mat3x3
    {
        public float[] entries = new float[9];

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < entries.Length; ++i)
            {
                builder.Append(entries[i]);
                builder.Append(" ");
            }

            return builder.ToString();
        }
    }

    public class Frame
    {
        public Vector3 m_fOrigin;
        public float qw = 1.0f;
        public float qx;
        public float qy;
        public float qz;
        public Mat3x3 m_fl2gv = new Mat3x3(); // Local-to-global matrix?
        public int Length;

        public void cache()
        {
            // TODO: Clean this up
            float b2 = qx + qx;
            float c2 = qy + qy;
            float v3 = qz + qz;
            float v4 = qw * b2;
            float v5 = qw * c2;
            float v6 = qw * v3;
            float v7 = qx * b2;
            float v8 = qx * c2;
            float bd2 = qx * v3;
            float cc2 = qy * c2;
            float v11 = qy * v3;
            float v12 = qz * v3;
            m_fl2gv.entries[0] = 1.0f - cc2 - v12;
            m_fl2gv.entries[1] = v8 + v6;
            m_fl2gv.entries[2] = bd2 - v5;
            m_fl2gv.entries[3] = v8 - v6;
            float v13 = 1.0f - v7;
            m_fl2gv.entries[4] = v13 - v12;
            m_fl2gv.entries[5] = v11 + v4;
            m_fl2gv.entries[6] = bd2 + v5;
            m_fl2gv.entries[7] = v11 - v4;
            m_fl2gv.entries[8] = v13 - cc2;
        }

        public static Frame read(BinaryReader binaryReader)
        {
            Frame newObj = new Frame();
            var startPosition = binaryReader.BaseStream.Position;
            newObj.m_fOrigin = Vector3.read(binaryReader);
            newObj.qw = binaryReader.ReadSingle();
            newObj.qx = binaryReader.ReadSingle();
            newObj.qy = binaryReader.ReadSingle();
            newObj.qz = binaryReader.ReadSingle();
            newObj.cache();
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }
    }

    public class PStringChar
    {
        public string m_buffer;
        public int Length;

        public static PStringChar read(BinaryReader binaryReader)
        {
            PStringChar newObj = new PStringChar();
            var startPosition = binaryReader.BaseStream.Position;
            uint size = binaryReader.ReadUInt16();
            if (size == ushort.MaxValue)
            {
                binaryReader.BaseStream.Seek(-2, SeekOrigin.Current);
                size = binaryReader.ReadUInt32();
            }

            if (size == 0)
            {
                newObj.m_buffer = null;
            }
            else
            {
                newObj.m_buffer = new string(binaryReader.ReadChars((int)size));
            }

            Util.readToAlign(binaryReader);
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }

        public override string ToString()
        {
            return m_buffer;
        }
    }

    public class PackableHashTable<TKey, TValue>
    {
        public Dictionary<TKey, TValue> hashTable = new Dictionary<TKey, TValue>();
        public int Length;

        public static PackableHashTable<TKey, TValue> read(BinaryReader binaryReader)
        {
            PackableHashTable<TKey, TValue> newObj = new PackableHashTable<TKey, TValue>();
            var startPosition = binaryReader.BaseStream.Position;
            uint sizeInfo = binaryReader.ReadUInt32();
            uint _table_size = sizeInfo >> 16; // NOTE: We don't actually need the bucket sizes since C# will just do its own thing internally
            uint _currNum = sizeInfo & 0xFFFF;
            for (int i = 0; i < _currNum; ++i)
            {
                newObj.hashTable.Add(Util.readers[typeof(TKey)](binaryReader), Util.readers[typeof(TValue)](binaryReader));
            }
            newObj.Length = (int)(binaryReader.BaseStream.Position - startPosition);
            return newObj;
        }
    }
}
