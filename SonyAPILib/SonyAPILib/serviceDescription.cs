using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonyAPILib
{

    #region IRCC
    /// <summary>
    /// IRCC:1 Service Description class
    /// </summary>

    [Serializable]
    public class ircc
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string friendlyServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string serviceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string controlURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string eventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string SCPDURL { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string sv_LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Status
        /// </summary>
        public string sv_CurrentStatus { get; set; }
    }
    #endregion

    #region AVTransport
    /// <summary>
    /// AVTransport:1 Service Description class
    /// </summary>
    [Serializable]
    public class avtransport
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string friendlyServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string serviceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string controlURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string eventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string SCPDURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Last Change variable
        /// </summary>
        public string sv_LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport State.
        /// </summary>
        public string sv_TransportState { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport Status.
        /// </summary>
        public string sv_TransportStatus { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Playback Storage Medium.
        /// </summary>
        public string sv_PlayBackStorageMedium { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Record Storage Medium.
        /// </summary>
        public string sv_RecordStorageMedium { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Possible Playback Storage Medium.
        /// </summary>
        public string sv_PossiblePlaybackStorageMedia { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Possible Record Storage Medium.
        /// </summary>
        public string sv_PossibleRecordStorageMedia { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current PLay Maode.
        /// </summary>
        public string sv_CurrentPlayMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport Play Speed.
        /// </summary>
        public int sv_TransportPlaySpeed { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Record Medium Write Status.
        /// </summary>
        public string sv_RecordMediumWriteStatus { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Record Quality Mode.
        /// </summary>
        public string sv_CurrentRecordQualityMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Possible Record Quality Modes.
        /// </summary>
        public string sv_PossibleRecordQualityModes { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Number of Tracks.
        /// </summary>
        public int sv_NumberOfTracks { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track.
        /// </summary>
        public int sv_CurrentTrack { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track Duration.
        /// </summary>
        public string sv_CurrentTrackDuration { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track Meta Data.
        /// </summary>
        public string sv_CurrentTrackMetaData { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track URI.
        /// </summary>
        public string sv_CurrentTrackURI { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the AVTransport URI.
        /// </summary>
        public string sv_AVTransportURI { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the AVTransport URI Meta Data.
        /// </summary>
        public string sv_AVTransportURIMetaData { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Next AVTransport URI.
        /// </summary>
        public string sv_NextAVTransportURI { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Next AVTransport URI Meta Data.
        /// </summary>
        public string sv_NextAVTransportURIMetaData { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Relative Time Position.
        /// </summary>
        public string sv_RelativeTimePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Absolute Time Position.
        /// </summary>
        public string sv_AbsoluteTimePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Relative Counter Position.
        /// </summary>
        public string sv_RelativeCounterPosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Absolute Counter POsition.
        /// </summary>
        public string sv_AbsoluteCounterPosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Transport Actions.
        /// </summary>
        public string sv_CurrentTransportActions { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Relative Byte Position.
        /// </summary>
        public string sv_X_DLNA_RelativeBytePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Absolute Byte Position.
        /// </summary>
        public string sv_X_DLNA_AbsoluteBytePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track Size.
        /// </summary>
        public string sv_X_DLNA_CurrentTrackSize { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Seek Mode.
        /// </summary>
        public string sv_A_ARG_TYPE_SeekMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Seek Target.
        /// </summary>
        public string sv_A_ARG_TYPE_SeekTarget { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Instance ID.
        /// </summary>
        public int sv_A_ARG_TYPE_InstanceID { get; set; }
    }
    #endregion

    #region Rendering Control
    /// <summary>
    /// Re4nderingControl:1 Service Description class
    /// </summary>
    [Serializable]
    public class renderingcontrol
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string friendlyServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string serviceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string controlURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string eventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string SCPDURL { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string sv_LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Preset Name List.
        /// </summary>
        public string sv_PresetNameList { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Mute State.
        /// </summary>
        public Boolean sv_Mute { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Volume state.
        /// </summary>
        public int sv_Volume { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Channel(Master).
        /// </summary>
        public string sv_Channel = "Master";
        /// <summary>
        /// Gets or Sets the State Variable for the Instance ID (0).
        /// </summary>
        public int sv_InstanceID = 0;
        /// <summary>
        /// Gets or Sets the State Variable for the Preset Name.
        /// </summary>
        public string sv_PresetName { get; set; }
    }
    #endregion

    #region Connection Managger
    /// <summary>
    /// ConnectionManager:1 Service Description class
    /// </summary>
    [Serializable]
    public class connectionmanager
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string friendlyServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string serviceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string controlURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string eventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string SCPDURL { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string sv_LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Connection Id.
        /// </summary>
        public int sv_ConnectionID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Connection Status.
        /// </summary>
        public string sv_ConnectionStatus { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Connection Manager.
        /// </summary>
        public string sv_ConnectionManager { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Direction.
        /// </summary>
        public string sv_Direction { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the RcsID.
        /// </summary>
        public string sv_RcsID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport ID.
        /// </summary>
        public int sv_AVTransportID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Peer Connection ID.
        /// </summary>
        public int sv_PeerConnectionID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Proticol Information.
        /// </summary>
        public string sv_ProtocolInfo { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Proticol Sink.
        /// </summary>
        public string sv_ProtocolSink { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Proticol Source.
        /// </summary>
        public string sv_ProticolSource { get; set; }
    }
    #endregion

    #region Party
    /// <summary>
    /// Party:1 Service Description class
    /// </summary>
    [Serializable]
    public class party
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string friendlyServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string serviceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string controlURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string eventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string SCPDURL { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string sv_LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Singer Capability.
        /// </summary>
        public int sv_SingerCapability { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport Port.
        /// </summary>
        public int sv_TransportPort { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Party State.
        /// </summary>
        public string sv_PartyState { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Party Mode.
        /// </summary>
        public string sv_PartyMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Party Song.
        /// </summary>
        public string sv_PartySong { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Session ID.
        /// </summary>
        public int sv_SessionID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Number of Listeners.
        /// </summary>
        public int sv_NumberOfListeners { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the ListenersList
        /// </summary>
        public string sv_ListenersList { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the UUID.
        /// </summary>
        public string sv_UUID { get; set; }
    }
    #endregion

    #region Action List

    /// <summary>
    /// IRCC:1 Service Description class
    /// </summary>
    [Serializable]
    public class ActionList
    {
        /// <summary>
        /// Gets or Sets the Action List Registration Mode
        /// </summary>
        public int RegisterMode { get; set; }
        /// <summary>
        /// Gets or Sets the Registration URL
        /// </summary>
        public string RegisterUrl { get; set; }
        /// <summary>
        /// Gets or Sets the System Information URL
        /// </summary>
        public string getSystemInformation { get; set; }
        /// <summary>
        /// Gets or Sets the Remote Commandlist URL
        /// </summary>
        public string getRemoteCommandList { get; set; }
        /// <summary>
        /// Gets or Sets the GetStatus URL
        /// </summary>
        public string getStatus { get; set; }
        /// <summary>
        /// Gets or Sets the GetText URL
        /// </summary>
        public string getText { get; set; }
        /// <summary>
        /// Gets or Sets the SendText URL
        /// </summary>
        public string sendText { get; set; }
    }
    #endregion

    #region Device Service
    /// <summary>
    /// Basic Service Description class
    /// </summary>
     
    [Serializable]
    public class deviceService
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string friendlyServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string serviceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string controlURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string eventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string SCPDURL { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string sv_LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Status
        /// </summary>
        public string sv_CurrentStatus { get; set; }
    }
    #endregion

}
