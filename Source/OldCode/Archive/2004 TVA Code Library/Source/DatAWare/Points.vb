'***********************************************************************
'  Points.vb - DatAWare Points Class
'  Copyright � 2005 - TVA, all rights reserved
'  
'  Build Environment: VB.NET, Visual Studio 2003
'  Primary Developer: James R Carroll, System Analyst [WESTAFF]
'      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
'       Phone: 423/751-2827
'       Email: jrcarrol@tva.gov
'
'  Code Modification History:
'  ---------------------------------------------------------------------
'  10/1/2004 - James R Carroll
'       Initial version of source created
'
'***********************************************************************
Option Explicit On 

Namespace DatAWare

    Public Class Points

        Private m_connection As Connection

        Friend Sub New(ByVal connection As Connection)

            m_connection = connection

        End Sub

        Public ReadOnly Property Count() As Integer
            Get
                VerifyOpenConnection()

                Dim pointCount As Integer
                Dim errorMessage As String

                With m_connection
                    .DWAPI.GetDBCount(.PlantCode, pointCount, errorMessage)

                    If Len(errorMessage) > 0 Then
                        Throw New InvalidOperationException("Failed to retrieve point count from DatAWare server """ & .Server & """ due to exception: " & errorMessage)
                    End If
                End With

                Return pointCount
            End Get
        End Property

        Public ReadOnly Property Definition(ByVal databaseIndex As Integer) As DatabaseStructure
            Get
                VerifyOpenConnection()

                Dim buffer As Byte() = Array.CreateInstance(GetType(Byte), DatabaseStructure.BinaryLength)
                Dim eof As Boolean
                Dim errorMessage As String

                With m_connection
                    .DWAPI.GetDBData(.PlantCode, databaseIndex, buffer, eof, errorMessage)

                    If eof Then
                        Throw New InvalidOperationException("Attempt to access database index passed end of file on DatAWare server """ & .Server & """")
                    ElseIf Len(errorMessage) > 0 Then
                        Throw New InvalidOperationException("Failed to retrieve point definition from DatAWare server """ & .Server & """ due to exception: " & errorMessage)
                    End If
                End With

                Return New DatabaseStructure(databaseIndex, buffer, 0)
            End Get
        End Property

        Default Public ReadOnly Property Value(ByVal databaseIndex As Integer, Optional ByVal timeRequest As String = "*", Optional ByVal timeInterval As Single = 0) As StandardEvent
            Get
                VerifyOpenConnection()

                Dim buffer As Byte() = Array.CreateInstance(GetType(Byte), ProcessEvent.BinaryLength)
                Dim errorMessage As String

                With m_connection
                    .DWAPI.ReadEvent(.PlantCode, databaseIndex, timeRequest, timeInterval, buffer, errorMessage)

                    If Len(errorMessage) > 0 Then
                        Throw New InvalidOperationException("Failed to retrieve point value from DatAWare server """ & .Server & """ due to exception: " & errorMessage)
                    End If
                End With

                Return New StandardEvent(databaseIndex, New ProcessEvent(buffer, 0))
            End Get
        End Property

        Public ReadOnly Property ValueRange(ByVal databaseIndex As Integer, ByVal startTimeRequest As String, Optional ByVal endTimeRequest As String = "*", Optional ByVal requestCommand As RequestType = RequestType.Raw, Optional ByVal timeInterval As Single = 0) As StandardEvent()
            Get
                VerifyOpenConnection()

                Dim events As StandardEvent()
                Dim buffer As Byte()
                Dim eventCount As Integer
                Dim errorMessage As String
                Dim index As Integer

                With m_connection
                    .DWAPI.ReadRange(.PlantCode, databaseIndex, startTimeRequest, endTimeRequest, buffer, requestCommand, timeInterval, eventCount, errorMessage)

                    If eventCount > 0 Then
                        events = Array.CreateInstance(GetType(StandardEvent), eventCount)

                        For x As Integer = 0 To eventCount - 1
                            events(x) = New StandardEvent(databaseIndex, New ProcessEvent(buffer, index))
                            index += ProcessEvent.BinaryLength
                        Next
                    End If
                End With

                Return events
            End Get
        End Property

        Private Sub VerifyOpenConnection()

            If Not m_connection.IsOpen Then Throw New InvalidOperationException("Operation unavailable when connection is closed.")

        End Sub

    End Class

End Namespace
