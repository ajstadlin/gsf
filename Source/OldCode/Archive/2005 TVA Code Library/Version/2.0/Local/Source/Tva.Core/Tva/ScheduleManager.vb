' 08-01-06

Imports System.Drawing
Imports System.ComponentModel
Imports System.Threading
Imports Tva.Services
Imports Tva.Configuration
Imports Tva.Configuration.Common

<ToolboxBitmap(GetType(ScheduleManager)), DefaultEvent("ProcessSchedule")> _
Public Class ScheduleManager
    Implements IServiceComponent

    Private m_configurationElement As String
    Private m_autoSaveSchedules As Boolean
    Private m_enabled As Boolean
    Private m_schedules As Dictionary(Of String, Schedule)
    Private m_startTimerThread As Thread
    Private WithEvents m_timer As System.Timers.Timer

    Public Event Starting As EventHandler
    Public Event Started As EventHandler
    Public Event Stopped As EventHandler
    Public Event CheckingSchedule(ByVal schedule As Schedule)
    Public Event ProcessSchedule(ByVal schedule As Schedule)

    Public Sub New(ByVal autoSaveSchedules As Boolean)
        MyBase.New()
        MyClass.ConfigurationElement = "ScheduleManager"
        MyClass.AutoSaveSchedules = autoSaveSchedules
        MyClass.Enabled = True
        m_schedules = New Dictionary(Of String, Schedule)()
        m_timer = New System.Timers.Timer(60000)
        LoadSchedules()
    End Sub

    ''' <summary>
    ''' Gets or sets the element name of the application configuration file under which the schedules will be saved.
    ''' </summary>
    ''' <value></value>
    ''' <returns>The element name of the application configuration file under which the schedules will be saved.</returns>
    Public Property ConfigurationElement() As String
        Get
            Return m_configurationElement
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                m_configurationElement = value
            Else
                Throw New ArgumentNullException("value")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a boolean value indicating whether the schedules will be saved automatically to the application
    ''' configuration file when this instance of Tva.ScheduleManager is stopped or disposed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property AutoSaveSchedules() As Boolean
        Get
            Return m_autoSaveSchedules
        End Get
        Set(ByVal value As Boolean)
            m_autoSaveSchedules = value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return m_enabled
        End Get
        Set(ByVal value As Boolean)
            m_enabled = value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property IsRunning() As Boolean
        Get
            Return m_timer.Enabled
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Schedules() As Dictionary(Of String, Schedule)
        Get
            Return m_schedules
        End Get
    End Property

    Public Sub Start()

        If m_enabled Then
            m_startTimerThread = New Thread(AddressOf StartTimer)
            m_startTimerThread.Start()
        End If

    End Sub

    Public Sub [Stop]()

        If m_enabled Then
            If m_startTimerThread IsNot Nothing Then m_startTimerThread.Abort()
            If m_timer.Enabled Then
                m_timer.Stop()
                If m_autoSaveSchedules Then SaveSchedules()
                RaiseEvent Stopped(Me, EventArgs.Empty)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Loads previously saved schedules from the application configuration file.
    ''' </summary>
    Public Sub LoadSchedules()

        If m_enabled Then
            For Each savedSchedule As CategorizedSettingsElement In DefaultConfigFile.CategorizedSettings(m_configurationElement)
                Dim schedule As New Schedule(savedSchedule.Name)
                schedule.Rule = savedSchedule.Value
                m_schedules(savedSchedule.Name) = schedule
            Next
        End If

    End Sub

    ''' <summary>
    ''' Saves all schedules to the application configuration file.
    ''' </summary>
    Public Sub SaveSchedules()

        If m_enabled Then
            DefaultConfigFile.CategorizedSettings(m_configurationElement).Clear()
            For Each scheduleName As String In m_schedules.Keys
                DefaultConfigFile.CategorizedSettings(m_configurationElement).Add(scheduleName, m_schedules(scheduleName).Rule)
            Next
            SaveSettings()
        End If

    End Sub

    Public Sub CheckSchedule(ByVal scheduleName As String)

        If m_enabled Then
            RaiseEvent CheckingSchedule(m_schedules(scheduleName))
            If m_schedules(scheduleName).IsDue() Then
                ThreadPool.QueueUserWorkItem(AddressOf AsynchronousProcessSchedule, scheduleName)
            End If
        End If

    End Sub

    Public Sub CheckAllSchedules()

        If m_enabled Then
            For Each scheduleName As String In m_schedules.Keys
                CheckSchedule(scheduleName)
            Next
        End If

    End Sub

    Private Sub AsynchronousProcessSchedule(ByVal state As Object)

        Dim scheduleName As String = Convert.ToString(state)
        RaiseEvent ProcessSchedule(m_schedules(scheduleName))

    End Sub

    Private Sub StartTimer()

        If Not m_timer.Enabled Then
            Do While True
                RaiseEvent Starting(Me, EventArgs.Empty)
                If System.DateTime.Now.Second = 0 Then
                    m_timer.Start()
                    RaiseEvent Started(Me, EventArgs.Empty)
                    CheckAllSchedules()
                    Exit Do
                End If
            Loop
        End If
        m_startTimerThread = Nothing

    End Sub

    Private Sub m_timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles m_timer.Elapsed

        CheckAllSchedules()

    End Sub

#Region " IServiceComponent Implementation "

    Private m_previouslyEnabled As Boolean = False

    Public ReadOnly Property Name() As String Implements Services.IServiceComponent.Name
        Get
            Return Me.GetType.Name
        End Get
    End Property

    Public Sub ProcessStateChanged(ByVal processName As String, ByVal newState As Services.ProcessState) Implements Services.IServiceComponent.ProcessStateChanged

    End Sub

    Public Sub ServiceStateChanged(ByVal newState As Services.ServiceState) Implements Services.IServiceComponent.ServiceStateChanged

        Select Case newState
            Case ServiceState.Paused
                m_previouslyEnabled = Enabled
                MyClass.Enabled = False
            Case ServiceState.Resumed
                MyClass.Enabled = m_previouslyEnabled
        End Select

    End Sub

    Public ReadOnly Property Status() As String Implements Services.IServiceComponent.Status
        Get
            With New System.Text.StringBuilder()
                .Append("       Number of schedules:")
                .Append(m_schedules.Count)
                .Append(Environment.NewLine)
                For Each scheduleName As String In m_schedules.Keys
                    .Append(m_schedules(scheduleName).Status)
                    .Append(Environment.NewLine)
                Next

                Return .ToString()
            End With
        End Get
    End Property

#End Region

End Class
