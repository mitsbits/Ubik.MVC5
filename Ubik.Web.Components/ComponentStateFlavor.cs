﻿using System;
using System.ComponentModel;

namespace Ubik.Web.Components
{
    [Flags]
    public enum ComponentStateFlavor : long
    {
        [Description("Empty is transient state")]
        Empty = 0,

        [Description("Persisted means that the entity is persisted to the data store")]
        Persisted = 0x00000001,

        [Description("Deleted means that the entity is unavailable to user ui")]
        Deleted = 0x00000002,

        //[Description("Assigned means that the entity is owned by a user")]
        //Assigned = 2 ^ 10,
        //[Description("PendingApproval means that the entity needs approval to get publish")]
        //PendingApproval = 2 ^ 11,
        //[Description("Approved means that the entity has been approved")]
        //Approved = 2 ^ 12,
        //[Description("Rejected means that the entity has been rejected")]
        //Rejected = 2 ^ 13,

        [Description("CheckedOut means that the entity is beeing edited")]
        CheckedOut = 0x00080000,

        [Description("Published means that the entity is publicly available")]
        Published = 0x20000000,

        [Description("Suspended means that the entity is publicly unavailable")]
        Suspended = 0x40000000
    }

    /*
     *
     *
        [Flags]
    public enum MyEnumeration : ulong
    {
        Flag1 = 0x00000001,
        Flag2 = 0x00000002,
        Flag3 = 0x00000004,
        Flag4 = 0x00000008,
        Flag5 = 0x00000010,
        Flag6 = 0x00000020,
        Flag7 = 0x00000040,
        Flag8 = 0x00000080,
        Flag9 = 0x00000100,
        Flag10 = 0x00000200,
        Flag11 = 0x00000400,
        Flag12 = 0x00000800,
        Flag13 = 0x00001000,
        Flag14 = 0x00002000,
        Flag15 = 0x00004000,
        Flag16 = 0x00008000,
        Flag17 = 0x00010000,
        Flag18 = 0x00020000,
        Flag19 = 0x00040000,
        Flag20 = 0x00080000,
        Flag21 = 0x00100000,
        Flag22 = 0x00200000,
        Flag23 = 0x00400000,
        Flag24 = 0x00800000,
        Flag25 = 0x01000000,
        Flag26 = 0x02000000,
        Flag27 = 0x04000000,
        Flag28 = 0x08000000,
        Flag29 = 0x10000000,
        Flag30 = 0x20000000,
        Flag31 = 0x40000000,
        Flag32 = 0x80000000,
        Flag33 = 0x100000000,
        Flag34 = 0x200000000,
        Flag35 = 0x400000000,
        Flag36 = 0x800000000,
        Flag37 = 0x1000000000,
        Flag38 = 0x2000000000,
        Flag39 = 0x4000000000,
        Flag40 = 0x8000000000,
        Flag41 = 0x10000000000,
        Flag42 = 0x20000000000,
        Flag43 = 0x40000000000,
        Flag44 = 0x80000000000,
        Flag45 = 0x100000000000,
        Flag46 = 0x200000000000,
        Flag47 = 0x400000000000,
        Flag48 = 0x800000000000,
        Flag49 = 0x1000000000000,
        Flag50 = 0x2000000000000,
        Flag51 = 0x4000000000000,
        Flag52 = 0x8000000000000,
        Flag53 = 0x10000000000000,
        Flag54 = 0x20000000000000,
        Flag55 = 0x40000000000000,
        Flag56 = 0x80000000000000,
        Flag57 = 0x100000000000000,
        Flag58 = 0x200000000000000,
        Flag59 = 0x400000000000000,
        Flag60 = 0x800000000000000,
        Flag61 = 0x1000000000000000,
        Flag62 = 0x2000000000000000,
        Flag63 = 0x4000000000000000,
        Flag64 = 0x8000000000000000,
    }
     */
}