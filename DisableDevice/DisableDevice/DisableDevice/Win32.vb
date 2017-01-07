Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Runtime.ConstrainedExecution
Imports System.Text


<Flags()> _
Friend Enum SetupDiGetClassDevsFlags
    [Default] = 1
    Present = 2
    AllClasses = 4
    Profile = 8
    DeviceInterface = &H10
End Enum

Friend Enum DiFunction
    SelectDevice = 1
    InstallDevice = 2
    AssignResources = 3
    Properties = 4
    Remove = 5
    FirstTimeSetup = 6
    FoundDevice = 7
    SelectClassDrivers = 8
    ValidateClassDrivers = 9
    InstallClassDrivers = &HA
    CalcDiskSpace = &HB
    DestroyPrivateData = &HC
    ValidateDriver = &HD
    Detect = &HF
    InstallWizard = &H10
    DestroyWizardData = &H11
    PropertyChange = &H12
    EnableClass = &H13
    DetectVerify = &H14
    InstallDeviceFiles = &H15
    UnRemove = &H16
    SelectBestCompatDrv = &H17
    AllowInstall = &H18
    RegisterDevice = &H19
    NewDeviceWizardPreSelect = &H1A
    NewDeviceWizardSelect = &H1B
    NewDeviceWizardPreAnalyze = &H1C
    NewDeviceWizardPostAnalyze = &H1D
    NewDeviceWizardFinishInstall = &H1E
    Unused1 = &H1F
    InstallInterfaces = &H20
    DetectCancel = &H21
    RegisterCoInstallers = &H22
    AddPropertyPageAdvanced = &H23
    AddPropertyPageBasic = &H24
    Reserved1 = &H25
    Troubleshooter = &H26
    PowerMessageWake = &H27
    AddRemotePropertyPageAdvanced = &H28
    UpdateDriverUI = &H29
    Reserved2 = &H30
End Enum

Friend Enum StateChangeAction
    Enable = 1
    Disable = 2
    PropChange = 3
    Start = 4
    [Stop] = 5
End Enum

<Flags()> _
Friend Enum Scopes
    [Global] = 1
    ConfigSpecific = 2
    ConfigGeneral = 4
End Enum

Friend Enum SetupApiError
    NoAssociatedClass = &HE0000200
    ClassMismatch = &HE0000201
    DuplicateFound = &HE0000202
    NoDriverSelected = &HE0000203
    KeyDoesNotExist = &HE0000204
    InvalidDevinstName = &HE0000205
    InvalidClass = &HE0000206
    DevinstAlreadyExists = &HE0000207
    DevinfoNotRegistered = &HE0000208
    InvalidRegProperty = &HE0000209
    NoInf = &HE000020A
    NoSuchHDevinst = &HE000020B
    CantLoadClassIcon = &HE000020C
    InvalidClassInstaller = &HE000020D
    DiDoDefault = &HE000020E
    DiNoFileCopy = &HE000020F
    InvalidHwProfile = &HE0000210
    NoDeviceSelected = &HE0000211
    DevinfolistLocked = &HE0000212
    DevinfodataLocked = &HE0000213
    DiBadPath = &HE0000214
    NoClassInstallParams = &HE0000215
    FileQueueLocked = &HE0000216
    BadServiceInstallSect = &HE0000217
    NoClassDriverList = &HE0000218
    NoAssociatedService = &HE0000219
    NoDefaultDeviceInterface = &HE000021A
    DeviceInterfaceActive = &HE000021B
    DeviceInterfaceRemoved = &HE000021C
    BadInterfaceInstallSect = &HE000021D
    NoSuchInterfaceClass = &HE000021E
    InvalidReferenceString = &HE000021F
    InvalidMachineName = &HE0000220
    RemoteCommFailure = &HE0000221
    MachineUnavailable = &HE0000222
    NoConfigMgrServices = &HE0000223
    InvalidPropPageProvider = &HE0000224
    NoSuchDeviceInterface = &HE0000225
    DiPostProcessingRequired = &HE0000226
    InvalidCOInstaller = &HE0000227
    NoCompatDrivers = &HE0000228
    NoDeviceIcon = &HE0000229
    InvalidInfLogConfig = &HE000022A
    DiDontInstall = &HE000022B
    InvalidFilterDriver = &HE000022C
    NonWindowsNTDriver = &HE000022D
    NonWindowsDriver = &HE000022E
    NoCatalogForOemInf = &HE000022F
    DevInstallQueueNonNative = &HE0000230
    NotDisableable = &HE0000231
    CantRemoveDevinst = &HE0000232
    InvalidTarget = &HE0000233
    DriverNonNative = &HE0000234
    InWow64 = &HE0000235
    SetSystemRestorePoint = &HE0000236
    IncorrectlyCopiedInf = &HE0000237
    SceDisabled = &HE0000238
    UnknownException = &HE0000239
    PnpRegistryError = &HE000023A
    RemoteRequestUnsupported = &HE000023B
    NotAnInstalledOemInf = &HE000023C
    InfInUseByDevices = &HE000023D
    DiFunctionObsolete = &HE000023E
    NoAuthenticodeCatalog = &HE000023F
    AuthenticodeDisallowed = &HE0000240
    AuthenticodeTrustedPublisher = &HE0000241
    AuthenticodeTrustNotEstablished = &HE0000242
    AuthenticodePublisherNotTrusted = &HE0000243
    SignatureOSAttributeMismatch = &HE0000244
    OnlyValidateViaAuthenticode = &HE0000245
End Enum

<StructLayout(LayoutKind.Sequential)> _
Friend Structure DeviceInfoData
    Public Size As Integer
    Public ClassGuid As Guid
    Public DevInst As Integer
    Public Reserved As IntPtr
End Structure

<StructLayout(LayoutKind.Sequential)> _
Friend Structure PropertyChangeParameters
    Public Size As Integer  ' part of header. It's flattened out into 1 structure.
    Public DiFunction As DiFunction
    Public StateChange As StateChangeAction
    Public Scope As Scopes
    Public HwProfile As Integer
End Structure

Friend Class NativeMethods

    Private Const setupapi As String = "setupapi.dll"

    Private Sub New()
    End Sub

    <DllImport(setupapi, CallingConvention:=CallingConvention.Winapi, SetLastError:=True)> _
    Public Shared Function SetupDiCallClassInstaller( _
    ByVal installFunction As DiFunction, _
    ByVal deviceInfoSet As SafeDeviceInfoSetHandle, _
    <[In]()> ByRef deviceInfoData As DeviceInfoData) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport(setupapi, CallingConvention:=CallingConvention.Winapi, SetLastError:=True)> _
    Public Shared Function SetupDiEnumDeviceInfo( _
    ByVal deviceInfoSet As SafeDeviceInfoSetHandle, _
    ByVal memberIndex As Integer, _
    ByRef deviceInfoData As DeviceInfoData) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport(setupapi, CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Unicode, SetLastError:=True)> _
    Public Shared Function SetupDiGetClassDevs( _
    <[In]()> ByRef classGuid As Guid, _
    <MarshalAs(UnmanagedType.LPWStr)> ByVal enumerator As String, _
    ByVal hwndParent As IntPtr, _
    ByVal flags As SetupDiGetClassDevsFlags) As SafeDeviceInfoSetHandle
    End Function

    <DllImport(setupapi, CallingConvention:=CallingConvention.Winapi, CharSet:=CharSet.Unicode, SetLastError:=True)> _
    Public Shared Function SetupDiGetDeviceInstanceId( _
    ByVal deviceInfoSet As SafeDeviceInfoSetHandle, _
    <[In]()> ByRef did As DeviceInfoData, _
    <MarshalAs(UnmanagedType.LPTStr)> ByVal _
    deviceInstanceId As StringBuilder, _
    ByVal deviceInstanceIdSize As Integer, _
    <Out()> ByRef requiredSize As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <SuppressUnmanagedCodeSecurity()> _
    <ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)> _
    <DllImport(setupapi, CallingConvention:=CallingConvention.Winapi, SetLastError:=True)> _
    Public Shared Function SetupDiDestroyDeviceInfoList( _
    ByVal deviceInfoSet As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport(setupapi, CallingConvention:=CallingConvention.Winapi, SetLastError:=True)> _
    Public Shared Function SetupDiSetClassInstallParams( _
    ByVal deviceInfoSet As SafeDeviceInfoSetHandle, _
    <[In]()> ByRef deviceInfoData As DeviceInfoData, _
    <[In]()> ByRef classInstallParams As PropertyChangeParameters, _
    ByVal classInstallParamsSize As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

End Class