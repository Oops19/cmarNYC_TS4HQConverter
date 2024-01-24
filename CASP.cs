/* Xmods Data Library, a library to support tools for The Sims 4,
   Copyright (C) 2014  C. Marinetti

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
   The author may be contacted at modthesims.info, username cmarNYC. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Xmods.DataLib
{
    public class CASP       //Sims 4 CASP resource
    {
        uint version;	// 0x2C is current
        uint offset;	// to resource reference table from end of header (ie offset + 8)
        int presetCount; // Not used for TS4
        string partname;		// UnicodeBE - part name
        float sortPriority;	// CAS sorts on this value, from largest to smallest

        ushort swatchOrder;   // mSecondaryDisplayIndex - swatch order
        uint outfitID;    // Used to group CASPs
        uint materialHash;
        byte parameterFlags;  //parameter flags: 
                                // 1 bit RestrictOppositeGender
                                // 1 bit AllowForLiveRandom
                                // 1 bit Show in CAS Demo
                                // 1 bit ShowInSimInfoPanel
                                // 1 bit ShowInUI
                                // 1 bit AllowForCASRandom : 1;
                                // 1 bit DefaultThumbnailPart
                                // 1 bit Deprecated
        byte parameterFlags2; // additional parameter flags:
                                // 5 bits unused
                                // 1 bit DefaultForBodyTypeFemale
                                // 1 bit DefaultForBodyTypeMale
                                // 1 bit RestrictOppositeFrame
        ulong excludePartFlags; // parts removed
        ulong excludePartFlags2;         // v0x29
        ulong excludeModifierRegionFlags;
        int tagCount;
        PartTag[] categoryTags; // [tagCount] PartTags
        uint price;             //deprecated
        uint titleKey;
        uint partDescKey;
        uint unknown;
        byte textureSpace;
        uint bodyType;
        uint bodySubType;    // usually 8, not used before version 37
        AgeGender ageGender;
        uint species;          // added in version 0x20
        ushort packID;       // added in version 0x25
        byte packFlags;      // added in version 0x25
                                // bit 7 - reserved, set to 0
                                // bit 1 - hide pack icon
        byte[] Reserved2Set0;  // added in version 0x25, nine bytes set to 0
        byte Unused2;        //usually 1
        byte Unused3;        //if Unused2 > 0; usually 0
        byte usedColorCount;
        uint[] colorData;    // [usedColorCount] color code
        byte buffResKey;     // index to data file with custom icon and text info
        byte swatchIndex;    // index to swatch image
        ulong VoiceEffect;   // added in version 0x1C -  mVoiceEffectHash, a hash of a sound effect
        byte usedMaterialCount;       // added in version 0x1e - if not 0, should be 3
        uint materialSetUpperBodyHash;       // added in version 0x1e
        uint materialSetLowerBodyHash;       // added in version 0x1e
        uint materialSetShoesHash;       // added in version 0x1e 
        uint occultBitField;            // added in version 0x1f - disabled for occult types
                                        // 30 bits reserved
                                        //  1 bit alien
                                        //  1 bit human
        ulong unknown1;                 // Version 0x2E
        ulong oppositeGenderPart;      // Version 0x28 - If the current part is not compatible with the Sim due to frame/gender
                                       // restrictions, use this part instead. Maxis convention is to use this
                                       // to specify the opposite gender version of the part. Set to 0 for none.
        ulong fallbackPart;            // Version 0x28 - If the current part is not compatible with the Sim due to frame/gender
                                       // restrictions, and there is no mOppositeGenderPart specified, use this part.
                                       // Maxis convention is to use this to specify a replacement part which is not
                                       // necessarily the opposite gendered version of the part. Set to 0 for none.
        OpacitySettings opacitySlider;     //V 0x2C
        SliderSettings hueSlider;           // "
        SliderSettings saturationSlider;    // "
        SliderSettings brightnessSlider;    // "
        byte unknown2;                      //Version 0x2E
        byte nakedKey;
        byte parentKey;
        int  sortLayer;    
        byte lodCount;
        MeshDesc[] lods;      // [count] mesh lod and part indexes 
        byte numSlotKeys;
        byte[] slotKeys;      // [numSlotKeys] bytes
        byte textureIndex;    // index to texture TGI (diffuse)
        byte shadowIndex;     // index to 'shadow' texture/overlay
        byte compositionMethod;
        byte regionMapIndex;  // index to RegionMap file
        byte numOverrides;
        Override[] overrides; // [numOverrides] Override
        byte normalMapIndex;
        byte specularIndex;   // DDSRLES 
        uint UVoverride;      //added in version 0x1b, so far same values as bodyType
        byte emissionIndex;   // added in version 0x1d, for alien glow 
        byte reserved;        // added in version 0x2A
        byte IGTcount;        // Resource reference table in I64GT format (not TGI64)
                              // --repeat(count)
        TGI[] IGTtable;

        public TGI[] LinkList
        {
            get { return IGTtable; }
            set { IGTtable = value; }
        }
        public int EmptyLink
        {
            get
            {
                TGI empty = new TGI(0, 0, 0);
                for (int i = 0; i < this.LinkList.Length; i++)
                {
                    if (this.LinkList[i].Equals(empty)) return i;
                }
                return -1;
            }
        }
        public byte TextureIndex
        {
            get { return this.textureIndex; }
            set { this.textureIndex = (byte)value; }
        }
        public byte ShadowIndex
        {
            get { return this.shadowIndex; }
            set { this.shadowIndex = (byte)value; }
        }
        public byte RegionMapIndex
        {
            get { return this.regionMapIndex; }
            set { this.regionMapIndex = (byte)value; }
        }
        public byte NormalMapIndex
        {
            get { return this.normalMapIndex; }
            set { this.normalMapIndex = (byte)value; }
        }
        public byte SpecularIndex
        {
            get { return this.specularIndex; }
            set { this.specularIndex = (byte)value; }
        }
        public byte EmissionIndex
        {
            get { if (this.version >= 30) { return this.emissionIndex; } else { return (byte)this.EmptyLink; } }
            set { this.emissionIndex = (byte)value; }
        }

        public CASP(BinaryReader br)
        {
            br.BaseStream.Position = 0;
            if (br.BaseStream.Length < 32) throw new CASPEmptyException("Attempt to read empty CASP");
            version = br.ReadUInt32();
            if (version == 0) throw new CASPEmptyException("Attempt to read zero version CASP");
            offset = br.ReadUInt32();
            presetCount = br.ReadInt32();
            byte tmpCountLow = br.ReadByte();
            int size = tmpCountLow;
            if (tmpCountLow > 127)
            {
                byte tmpCountHigh = br.ReadByte();
                size = (tmpCountHigh << 7) | (tmpCountLow & 0x7F);
            }
            byte[] tmp = br.ReadBytes(size);
            partname = System.Text.Encoding.BigEndianUnicode.GetString(tmp);
            //partname.ToCharArray();
            sortPriority = br.ReadSingle();
            swatchOrder = br.ReadUInt16();
            outfitID = br.ReadUInt32();
            materialHash = br.ReadUInt32();
            parameterFlags = br.ReadByte();
            if (this.version >= 39) parameterFlags2 = br.ReadByte();
            excludePartFlags = br.ReadUInt64();
            if (version >= 41)
            {
                excludePartFlags2 = br.ReadUInt64();
            }
            if (version > 36)
            {
                excludeModifierRegionFlags = br.ReadUInt64();
            }
            else
            {
                excludeModifierRegionFlags = br.ReadUInt32();
            }
            tagCount = br.ReadInt32();
            categoryTags = new PartTag[tagCount];
            for (int i = 0; i < tagCount; i++)
            {
                categoryTags[i] = new PartTag(br, version >= 37 ? 4 : 2);
            }
            price = br.ReadUInt32();
            titleKey = br.ReadUInt32();
            partDescKey = br.ReadUInt32();
            if (version >= 43)
            {
                unknown = br.ReadUInt32();
            }
            textureSpace = br.ReadByte();
            bodyType = br.ReadUInt32();
            bodySubType = br.ReadUInt32();
            ageGender = (AgeGender)br.ReadUInt32();
            if (this.version >= 32) species = br.ReadUInt32();
            if (this.version >= 34)
            {
                packID = br.ReadUInt16();
                packFlags = br.ReadByte();
                Reserved2Set0 = br.ReadBytes(9);
            }
            else
            {
                Unused2 = br.ReadByte();
                if (Unused2 > 0) Unused3 = br.ReadByte();
            }
            usedColorCount = br.ReadByte();
            colorData = new uint[usedColorCount];
            for (int i = 0; i < usedColorCount; i++)
            {
                colorData[i] = br.ReadUInt32();
            }
            buffResKey = br.ReadByte();
            swatchIndex = br.ReadByte();
            if (version >= 28)
            {
                VoiceEffect = br.ReadUInt64();
            }
            if (version >= 30)
            {
                usedMaterialCount = br.ReadByte();
                if (usedMaterialCount > 0)
                {
                    materialSetUpperBodyHash = br.ReadUInt32();
                    materialSetLowerBodyHash = br.ReadUInt32();
                    materialSetShoesHash = br.ReadUInt32();
                }
            }
            if (version >= 31)
            {
                occultBitField = br.ReadUInt32();
            }
            if (version >= 0x2E)
            {
                unknown1 = br.ReadUInt64();
            }
            if (version >= 38)
            {
                oppositeGenderPart = br.ReadUInt64();
            }
            if(version >= 39)
            {
                fallbackPart = br.ReadUInt64();
            }
            if (version >= 44)
            {
                opacitySlider = new OpacitySettings(br);
                hueSlider = new SliderSettings(br);
                saturationSlider = new SliderSettings(br);
                brightnessSlider = new SliderSettings(br);
            }
            if (version >= 0x2E)
            {
                unknown2 = br.ReadByte();
            }
            nakedKey = br.ReadByte();
            parentKey = br.ReadByte();
            sortLayer = br.ReadInt32();
            lodCount = br.ReadByte();
            lods = new MeshDesc[lodCount];
            for (int i = 0; i < lodCount; i++)
            {
                lods[i] = new MeshDesc(br);
            }
            numSlotKeys = br.ReadByte();
            slotKeys = br.ReadBytes(numSlotKeys);
            textureIndex = br.ReadByte(); 
            shadowIndex = br.ReadByte(); 
            compositionMethod = br.ReadByte();
            regionMapIndex = br.ReadByte(); 
            numOverrides = br.ReadByte();
            overrides = new Override[numOverrides];
            for (int i = 0; i < numOverrides; i++)
            {
                overrides[i] = new Override(br);
            }
            normalMapIndex = br.ReadByte();
            specularIndex = br.ReadByte();
            if (version >= 27)
            {
                UVoverride = br.ReadUInt32();
            }
            if (version >= 29)
            {
                emissionIndex = br.ReadByte();
            }
            if (version >= 42)
            {
                reserved = br.ReadByte();
            }
            IGTcount = br.ReadByte();
            IGTtable = new TGI[IGTcount];
            for (int i = 0; i < IGTcount; i++)
            {
                IGTtable[i] = new TGI(br, TGI.TGIsequence.IGT);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(version);
            long offsetPos = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(presetCount);
            int size = partname.Length * 2;
            if (size > 127)
            {
                unchecked
                {
                    bw.Write((byte)(size));
                }
                bw.Write((byte)(size >> 7));
            }
            else
            {
                bw.Write((byte)size);
            }
            byte[] str = System.Text.Encoding.ASCII.GetBytes(partname);
            for (int i = 0; i < partname.Length; i++)
            {
                bw.Write((byte)0);
                bw.Write(str[i]);
            }
            bw.Write(sortPriority);
            bw.Write(swatchOrder);
            bw.Write(outfitID);
            bw.Write(materialHash);
            bw.Write(parameterFlags);
            if (this.version >= 39) bw.Write(parameterFlags2);
            bw.Write(excludePartFlags);
            if (version >= 41)
            {
                bw.Write(excludePartFlags2);
            }
            if (version > 36)
            {
                bw.Write(excludeModifierRegionFlags);
            }
            else
            {
                bw.Write((uint)excludeModifierRegionFlags);
            }
            bw.Write(tagCount);
            for (int i = 0; i < tagCount; i++)
            {
                categoryTags[i].Write(bw, version >= 37 ? 4 : 2);
            }
            bw.Write(price);
            bw.Write(titleKey);
            bw.Write(partDescKey);
            if (version >= 43)
            {
                bw.Write(unknown);
            }
            bw.Write(textureSpace);
            bw.Write(bodyType);
            bw.Write(bodySubType);
            bw.Write((uint)ageGender);
            if (this.version >= 32) bw.Write(species);
            if (this.version >= 34)
            {
                bw.Write(packID);
                bw.Write(packFlags);
                bw.Write(Reserved2Set0);
            }
            else
            {
                bw.Write(Unused2);
                if (Unused2 > 0) bw.Write(Unused3);
            }
            bw.Write(usedColorCount);
            for (int i = 0; i < usedColorCount; i++)
            {
                bw.Write(colorData[i]);
            }
            bw.Write(buffResKey);
            bw.Write(swatchIndex);
            if (version >= 28)
            {
                bw.Write(VoiceEffect);
            }
            if (version >= 30)
            {
                bw.Write(usedMaterialCount);
                if (usedMaterialCount > 0)
                {
                     bw.Write(materialSetUpperBodyHash);
                     bw.Write(materialSetLowerBodyHash);
                     bw.Write(materialSetShoesHash);
                }
            }
            if (version >= 31)
            {
                bw.Write(occultBitField);
            }
            if (version >= 0x2E)
            {
                bw.Write(unknown1);
            }
            if (version >= 38)
            {
                 bw.Write(oppositeGenderPart);
            }
            if (version >= 39)
            {
                 bw.Write(fallbackPart);
            }
            if (version >= 44)
            {
                opacitySlider.Write(bw);
                hueSlider.Write(bw);
                saturationSlider.Write(bw);
                brightnessSlider.Write(bw);
            }
            if (version >= 0x2E)
            {
                bw.Write(unknown2);
            }
            bw.Write(nakedKey);
            bw.Write(parentKey);
            bw.Write(sortLayer);
            bw.Write(lodCount);
            for (int i = 0; i < lodCount; i++)
            {
                lods[i].Write(bw);
            }
            bw.Write(numSlotKeys);
            bw.Write(slotKeys);
            bw.Write(textureIndex);
            bw.Write(shadowIndex);
            bw.Write(compositionMethod);
            bw.Write(regionMapIndex);
            bw.Write(numOverrides);
            for (int i = 0; i < numOverrides; i++)
            {
                overrides[i].Write(bw);
            }
            bw.Write(normalMapIndex);
            bw.Write(specularIndex);
            if (version >= 27)
            {
                bw.Write(UVoverride);
            }
            if (version >= 29)
            {
                 bw.Write(emissionIndex);
            }
            if (version >= 42)
            {
                bw.Write(reserved);
            }
            long tablePos = bw.BaseStream.Position;
            bw.BaseStream.Position = offsetPos;
            bw.Write((uint)(tablePos - 8));
            bw.BaseStream.Position = tablePos;
            bw.Write((byte)IGTtable.Length);
            for (int i = 0; i < IGTtable.Length; i++)
            {
                IGTtable[i].Write(bw, TGI.TGIsequence.IGT);
            }
        }

        internal class PartTag
        {
            ushort flagCategory;
            uint flagValue;

            internal ushort FlagCategory
            {
                get { return this.flagCategory; }
                set { this.flagCategory = value; }
            }
            internal uint FlagValue
            {
                get { return this.flagValue; }
                set { this.flagValue = value; }
            }

            internal PartTag(BinaryReader br, int valueLength)
            {
                flagCategory = br.ReadUInt16();
                if (valueLength == 4)
                {
                    flagValue = br.ReadUInt32();
                }
                else
                {
                    flagValue = br.ReadUInt16();
                }
            }

            internal PartTag(ushort category, uint flagVal)
            {
                this.flagCategory = category;
                this.flagValue = flagVal;
            }

            internal void Write(BinaryWriter bw, int valueLength)
            {
                bw.Write(flagCategory);
                if (valueLength == 4)
                {
                    bw.Write(flagValue);
                }
                else
                {
                    bw.Write((ushort)flagValue);
                }
            }
        }

        internal class Override
        {
            byte region;
            float layer;

            internal Override(BinaryReader br)
            {
                region = br.ReadByte();
                layer = br.ReadSingle();
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(region);
                bw.Write(layer);
            }
        }

        internal class MeshDesc
        {
            internal byte lod;
            internal uint Unused1;
            LODasset[] assets;
            internal byte[] indexes;

            internal int Length
            {
                get
                {
                    return 7 + (12 * assets.Length) + indexes.Length;
                }
            }

            internal void removeMeshLink(TGI meshTGI, TGI[] caspTGIlist)
            {
                List<byte> tmp = new List<byte>();
                foreach (byte i in indexes)
                {
                    if (!meshTGI.Equals(caspTGIlist[i])) tmp.Add(i);
                }
                this.indexes = tmp.ToArray();
            }

            internal void addMeshLink(TGI meshTGI, TGI[] caspTGIlist)
            {
                List<byte> tmp = new List<byte>(this.indexes);
                for (byte i = 0; i < caspTGIlist.Length; i++)
                {
                    if (meshTGI.Equals(caspTGIlist[i]))
                    {
                        tmp.Add(i);
                        break;
                    }
                }
                this.indexes = tmp.ToArray();
            }

            internal MeshDesc(BinaryReader br)
            {
                lod = br.ReadByte();
                Unused1 = br.ReadUInt32();
                byte numAssets = br.ReadByte();
                assets = new LODasset[numAssets];
                for (int i = 0; i < numAssets; i++)
                {
                    assets[i] = new LODasset(br);
                }
                byte indexCount = br.ReadByte();
                indexes = new byte[indexCount];
                for (int i = 0; i < indexCount; i++)
                {
                    indexes[i] = br.ReadByte();
                }
            }

            internal void Write(BinaryWriter bw)
            {
                bw.Write(lod);
                bw.Write(Unused1);
                bw.Write((byte)assets.Length);
                for (int i = 0; i < assets.Length; i++)
                {
                    assets[i].Write(bw);
                }
                bw.Write((byte)indexes.Length);
                for (int i = 0; i < indexes.Length; i++)
                {
                    bw.Write(indexes[i]);
                }
            }

            internal class LODasset
            {
                internal int sorting;
                internal int specLevel;
                internal int castShadow;

                internal LODasset(BinaryReader br)
                {
                    this.sorting = br.ReadInt32();
                    this.specLevel = br.ReadInt32();
                    this.castShadow = br.ReadInt32();
                }

                internal void Write(BinaryWriter bw)
                {
                    bw.Write(this.sorting);
                    bw.Write(this.specLevel);
                    bw.Write(this.castShadow);
                }
            }
        }

        public class OpacitySettings
        {
            internal float minimum;
            internal float increment;

            public float Minimum
            {
                get { return this.minimum; }
                set { this.minimum = value; }
            }
            public float Increment
            {
                get { return this.increment; }
                set { this.increment = value; }
            }

            public OpacitySettings()
            {
                this.minimum = .2f;
                this.increment = .05f;
            }

            public OpacitySettings(float minimum, float increment)
            {
                this.minimum = minimum;
                this.increment = increment;
            }

            internal OpacitySettings(BinaryReader br)
            {
                this.minimum = br.ReadSingle();
                this.increment = br.ReadSingle();
            }

            internal virtual void Write(BinaryWriter bw)
            {
                bw.Write(this.minimum);
                bw.Write(this.increment);
            }
        }

        public class SliderSettings : OpacitySettings
        {
            internal float maximum;

            public float Maximum
            {
                get { return this.maximum; }
                set { this.maximum = value; }
            }

            public SliderSettings()
            {
                this.minimum = -.5f;
                this.maximum = .5f;
                this.increment = .05f;
            }

            public SliderSettings(float minimum, float maximum, float increment)
            {
                this.minimum = minimum;
                this.maximum = maximum;
                this.increment = increment;
            }

            internal SliderSettings(BinaryReader br)
            {
                this.minimum = br.ReadSingle();
                this.maximum = br.ReadSingle();
                this.increment = br.ReadSingle();
            }

            internal override void Write(BinaryWriter bw)
            {
                bw.Write(this.minimum);
                bw.Write(this.maximum);
                bw.Write(this.increment);
            }
        }

        public enum AgeGender : uint
        {
            Baby = 0x00000001,
            Toddler = 0x00000002,
            Child = 0x00000004,
            Teen = 0x00000008,
            YoungAdult = 0x00000010,
            Adult = 0x00000020,
            Elder = 0x00000040,
            Male = 0x00001000,
            Female = 0x00002000
        }


        [global::System.Serializable]
        public class CASPEmptyException : ApplicationException
        {
            public CASPEmptyException() { }
            public CASPEmptyException(string message) : base(message) { }
            public CASPEmptyException(string message, Exception inner) : base(message, inner) { }
            protected CASPEmptyException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }

    }
}
