Namespace Packets

    Public Interface IPacket

        Property ArchiveFile() As Files.DwArchiveFile

        Property MetadataFile() As Files.DwMetadataFile

        Property ActionType() As PacketActionType

        ReadOnly Property ReplyData() As Byte()

        Sub SaveData()

    End Interface

End Namespace