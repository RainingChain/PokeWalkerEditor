namespace PokeWalkerEditor.Core;

using PKHeX.Core;
using System;
using static System.Buffers.Binary.BinaryPrimitives;

class SAV4PW_ForPkHex : SaveFile
{

    public override string Extension => ".eeprom";
    public override bool ChecksumsValid => true;
    public override string ChecksumInfo => "";
    public override int Generation => 4;
    public override EntityContext Context => EntityContext.Gen4;
    public override IPersonalTable Personal => PersonalTable.HGSS;
    public override int MaxStringLengthOT => 7;
    public override int MaxStringLengthNickname => 10;
    public sealed override ushort MaxMoveID => Legal.MaxMoveID_4;
    public sealed override ushort MaxSpeciesID => Legal.MaxSpeciesID_4;
    public sealed override int MaxAbilityID => Legal.MaxAbilityID_4;
    public override int MaxItemID => Legal.MaxItemID_4_HGSS;
    public sealed override int MaxBallID => Legal.MaxBallID_4;
    public sealed override int MaxGameID => Legal.MaxGameID_4; // Colo/XD
    public override int BoxCount => 0;

    public sealed override Type PKMType => typeof(PK4);
    public sealed override PK4 BlankPKM => new();
    public sealed override int MaxEV => 255;


    public override ReadOnlySpan<ushort> HeldItems => Legal.HeldItems_HGSS;

    protected sealed override int SIZE_STORED => PokeCrypto.SIZE_4STORED;
    protected sealed override int SIZE_PARTY => PokeCrypto.SIZE_4PARTY;
    protected internal override string ShortSummary => $"{OT} ({Version}) - {PlayTimeString}";
    public override string GetBoxName(int box) => "";
    public override int GetBoxOffset(int box) => 0;
    public override int GetPartyOffset(int slot) => 0;
    public sealed override string GetString(ReadOnlySpan<byte> data)
        {
        return PWEncoding.GetString(data);
        }

    public override void SetBoxName(int box, ReadOnlySpan<char> value)
    {
        // nothing
    }
    public override int SetString(Span<byte> destBuffer, ReadOnlySpan<char> value, int maxLength, StringConverterOption option)
    {
        throw new NotImplementedException();
    }

    protected override SaveFile CloneInternal()
    {
        throw new NotImplementedException();
    }

    protected override byte[] DecryptPKM(byte[] data)
    {
        throw new NotImplementedException();
    }

    protected override PKM GetPKM(byte[] data)
    {
        throw new NotImplementedException();
    }

    protected override void SetChecksums()
    {
        //TODO
    }
}

public ref struct Important
{
    private byte[] Data;
    readonly int SIZE = 0x100;

    private int identity_offset => 0x6D;
    private int identity_size => 0x68;
    private int identity_checksum_offset => identity_offset + identity_size;

    private int health_offset => 0xd6;
    private int health_size => 0x18;
    private int health_checksum_offset => health_offset + health_size;

    public Important(ReadOnlySpan<byte> data, int offset)
    {
        Data = data.Slice(offset, SIZE).ToArray();
    }

    public byte[] Write()
    {
        Data[identity_checksum_offset] = Utils.Checksum(Data.AsSpan(identity_offset, identity_size));
        Data[health_checksum_offset] = Utils.Checksum(Data.AsSpan(health_offset, health_size));
        return Data;
    }

    

    public ushort TID16
    {
        get => ReadUInt16BigEndian(Data.AsSpan(identity_offset + 0xC)); // NO_PROD ReadUInt16LittleEndian
        set => WriteUInt16BigEndian(Data.AsSpan(identity_offset + 0xC), value);
    }

    public ushort SID16
    {
        get => ReadUInt16BigEndian(Data.AsSpan(identity_offset + 0xE)); // NO_PROD ReadUInt16LittleEndian
        set => WriteUInt16BigEndian(Data.AsSpan(identity_offset + 0xE), value); // NO_PROD ReadUInt16LittleEndian
    }

    public string OT
    {
        get => PWEncoding.GetString(Data.Slice(identity_offset + 0x48, 8 * 2));
        set => PWEncoding.SetString(Data.Slice(identity_offset + 0x48, 8 * 2), value, 16);
    }

    public uint LastSyncTime
    {
        get => ReadUInt32BigEndian(Data.AsSpan(identity_offset + 0x60)); // NO_PROD ReadUInt16LittleEndian
        set => WriteUInt32BigEndian(Data.AsSpan(identity_offset + 0x60), value);
    }
    public uint IdentitySpec_StepCount
    {
        get => ReadUInt32BigEndian(Data.AsSpan(identity_offset + 0x64)); // NO_PROD ReadUInt16LittleEndian
        set => WriteUInt32BigEndian(Data.AsSpan(identity_offset + 0x64), value);
    }

    public uint LifetimeStepCount
    {
        get => ReadUInt32BigEndian(Data.AsSpan(health_offset + 0x0));
        set => WriteUInt32BigEndian(Data.AsSpan(health_offset + 0x0), value);
    }

    public uint TodayStepCount
    {
        get => ReadUInt32BigEndian(Data.AsSpan(health_offset + 0x4));
        set => WriteUInt32BigEndian(Data.AsSpan(health_offset + 0x4), value);
    }

    public ushort CurrentWatts
    {
        get => ReadUInt16BigEndian(Data.AsSpan(health_offset + 0xe));
        set => WriteUInt16BigEndian(Data.AsSpan(health_offset + 0xe), value);
    }
}

internal sealed class SAV4PW_Offsets
{
    public int nintendo => 0x0;
    public int unk1 => 0x8;
    public int unk2 => 0x10;
    public int numResets => 0x72;
    public int unk3 => 0x73;
    public int important1 => 0x80;
    public int important2 => 0x180;
    public int sprites => 0x280;
    public int unk4 => 0x8C70;
    public int randomCheck => 0x8CB0;
    public int routeInfo => 0x8F00;
    public int areaSprite => 0x8FBE;
    public int areaNameSprite => 0x907E;
    public int walkPokeAnimatedSpritesSmall => 0x91BE;
    public int walkPokeAnimatedSpriteLarge => 0x933E;
    public int walkPokeNameSprite => 0x993E;
    public int routePokeSprites => 0x9A7E;
    public int joinPokeAnimatedSprite => 0x9EFE;
    public int routePokeNameSprites => 0xA4FE;
    public int itemNameSprites => 0xA8BE;
    public int unk5 => 0xB7BE;
    public int receivedSet => 0xB800;
    public int unused3 => 0xB801;
    public int specialMap => 0xB804;
    public int eventstuff => 0xBA44;
    public int unused5 => 0xBEC8;
    public int specialRoute => 0xBF00;
    public int unused4 => 0xCBBC;
    public int team => 0xCC00;
    public int pad1 => 0xCE24;
    public int unused => 0xCE80;
    public int giveStarf => 0xCE88;
    public int unused2 => 0xCE89;
    public int wattsForRemote => 0xCE8A;
    public int caughtPokes => 0xCE8C;
    public int dowsedItems => 0xCEBC;
    public int giftedItems => 0xCEC8;
    public int stepsHistory => 0xCEF0;
    public int eventLog => 0xCF0C;
    public int padd => 0xDBCC;
    public int peer => 0xDC00;
    public int metPeers => 0xDE24;
    public int unused6 => 0xF38C;
    public int peerPlayData => 0xF400;

}

class ReceivedIcons
{
    byte Data = 0;

    public ReceivedIcons(byte data) => Data = data;

    public byte Write() => Data;

    public bool Heart
    {
        get => (Data & 0b1) != 0;
        set => Data |= 0b1;
    }

    public bool Spade
    {
        get => (Data & 0b10) != 0;
        set => Data |= 0b10;
    }
    public bool Diamond
    {
        get => (Data & 0b100) != 0;
        set => Data |= 0b100;
    }

    public bool Club
    {
        get => (Data & 0b1000) != 0;
        set => Data |= 0b1000;
    }

    public bool SpecialMap
    {
        get => (Data & 0b10000) != 0;
        set => Data |= 0b10000;
    }

    public bool EventPkm
    {
        get => (Data & 0b100000) != 0;
        set => Data |= 0b100000;
    }

    public bool EventItem
    {
        get => (Data & 0b1000000) != 0;
        set => Data |= 0b1000000;
    }

    public bool SpecialRoute
    {
        get => (Data & 0b10000000) != 0;
        set => Data |= 0b10000000;
    }
}


struct PokemonSummary
{
    public const int SIZE = 0x10;
    private byte[] Data;

    public PokemonSummary(ReadOnlySpan<byte> data, int offset)
    {
        Data = data.Slice(offset, SIZE).ToArray();
    }

    public byte[] Write() => Data;

    public ushort Species
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(0x0));
        set => WriteUInt16LittleEndian(Data.AsSpan(0x0), value);
    }

    public ushort HeldItem
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(0x2));
        set => WriteUInt16LittleEndian(Data.AsSpan(0x2), value);
    }

    public ushort[] Moves
    {
        get => new ushort[] { Move1, Move2, Move3, Move4 };
        set
        {
            if (value.Length == 4)
                (Move1, Move2, Move3, Move4) = (value[0], value[1], value[2], value[3]);
        }
    }

    public ushort Move1
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(0x4));
        set => WriteUInt16LittleEndian(Data.AsSpan(0x4), value);
    }
    public ushort Move2
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(0x6));
        set => WriteUInt16LittleEndian(Data.AsSpan(0x6), value);
    }
    public ushort Move3
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(0x8));
        set => WriteUInt16LittleEndian(Data.AsSpan(0x8), value);
    }
    public ushort Move4
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(0xA));
        set => WriteUInt16LittleEndian(Data.AsSpan(0xA), value);
    }

    public byte Level
    {
        get => Data[0x8];
        set => Data[0x8] = value;
    }
    public byte VariantAndFlags
    {
        get => Data[0x9];
        set => Data[0x9] = value;
    }
    public byte MoveFlags
    {
        get => Data[0xA];
        set => Data[0xA] = value;
    }
}

public static class Utils
{
    public static byte Checksum(Span<byte> span)
    {
        byte checksum = 0;
        foreach (byte b in span)
            checksum += b;
        return checksum;
    }
}

class SAV4PW : SAV4PW_ForPkHex
{

    public SAV4PW(byte[] data) 
    { 
        Data = data; 
    }

    SAV4PW_Offsets Offsets = new();

    public Important Important
    {
        get => new(Data, Offsets.important1);
        set
        {
            var bytes = value.Write();
            SetData(Data.AsSpan(Offsets.important1), bytes);
            SetData(Data.AsSpan(Offsets.important2), bytes);
        }
    }

    protected Important Important2
    {
        get => new(Data, Offsets.important2);
    }

    public bool GiveStarfBerryAt99999Steps
    {
        get => Data[Offsets.giveStarf] != 0;
        set => Data[Offsets.giveStarf] = (byte)(value ? 1 : 0);
    }

    public ReceivedIcons ReceivedIcons
    {
        get => new ReceivedIcons(Data[Offsets.receivedSet]);
        set => Data[Offsets.receivedSet] = value.Write();
    }


    public PokemonSummary[] CaughtMons
    {
        get => new PokemonSummary[] { CaughtMon1, CaughtMon2, CaughtMon3 };
        set
        {
            if (value.Length == 3)
                (CaughtMon1, CaughtMon2, CaughtMon3) = (value[0], value[1], value[2]);
        }
    }

    public PokemonSummary CaughtMon1
    {
        get => new(Data, Offsets.caughtPokes);
        set => SetData(Data.AsSpan(Offsets.caughtPokes), value.Write());
    }
    public PokemonSummary CaughtMon2
    {
        get => new(Data, Offsets.caughtPokes + PokemonSummary.SIZE);
        set => SetData(Data.AsSpan(Offsets.caughtPokes + PokemonSummary.SIZE), value.Write());
    }
    public PokemonSummary CaughtMon3
    {
        get => new(Data, Offsets.caughtPokes + 2 * PokemonSummary.SIZE);
        set => SetData(Data.AsSpan(Offsets.caughtPokes + 2 * PokemonSummary.SIZE), value.Write());
    }

}

