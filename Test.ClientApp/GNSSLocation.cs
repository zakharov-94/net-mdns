using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable, DesignerCategory("code"), XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true), GeneratedCode("xsd", "2.0.50727.3038"), DebuggerStepThrough]
public class GNSSLocation
{
    private string[] dataField;
    private Latitude latitudeField;
    private Longitude longitudeField;
    private double altitudeField;
    private bool altitudeFieldSpecified;
    private DateTime timeField;
    private bool timeFieldSpecified;
    private DateTime dateField;
    private bool dateFieldSpecified;
    private double speedOverGroundField;
    private bool speedOverGroundFieldSpecified;
    private SignalQuality signalQualityField;
    private bool signalQualityFieldSpecified;
    private Fix fixField;
    private bool fixFieldSpecified;
    private int numberOfSatellitesField;
    private double horizontalDilutionOfPrecisionField;
    private bool horizontalDilutionOfPrecisionFieldSpecified;
    private double verticalDilutionOfPrecisionField;
    private bool verticalDilutionOfPrecisionFieldSpecified;
    private double trackDegreeTrueField;
    private bool trackDegreeTrueFieldSpecified;
    private double trackDegreeMagneticField;
    private bool trackDegreeMagneticFieldSpecified;
    private GNSSType gNSSTypeField;
    private GNSSCoordinateSystem gNSSCoordinateSystemField;
    private bool gNSSCoordinateSystemFieldSpecified;

    [XmlElement("Data")]
    public string[] Data
    {
        get
        {
            return this.dataField;
        }
        set
        {
            this.dataField = value;
        }
    }

    public Latitude Latitude
    {
        get
        {
            return this.latitudeField;
        }
        set
        {
            this.latitudeField = value;
        }
    }

    public Longitude Longitude
    {
        get
        {
            return this.longitudeField;
        }
        set
        {
            this.longitudeField = value;
        }
    }

    public double Altitude
    {
        get
        {
            return this.altitudeField;
        }
        set
        {
            this.altitudeField = value;
        }
    }

    [XmlIgnore]
    public bool AltitudeSpecified
    {
        get
        {
            return this.altitudeFieldSpecified;
        }
        set
        {
            this.altitudeFieldSpecified = value;
        }
    }

    [XmlElement(DataType = "time")]
    public DateTime Time
    {
        get
        {
            return this.timeField;
        }
        set
        {
            this.timeField = value;
        }
    }

    [XmlIgnore]
    public bool TimeSpecified
    {
        get
        {
            return this.timeFieldSpecified;
        }
        set
        {
            this.timeFieldSpecified = value;
        }
    }

    [XmlElement(DataType = "date")]
    public DateTime Date
    {
        get
        {
            return this.dateField;
        }
        set
        {
            this.dateField = value;
        }
    }

    [XmlIgnore]
    public bool DateSpecified
    {
        get
        {
            return this.dateFieldSpecified;
        }
        set
        {
            this.dateFieldSpecified = value;
        }
    }

    public double SpeedOverGround
    {
        get
        {
            return this.speedOverGroundField;
        }
        set
        {
            this.speedOverGroundField = value;
        }
    }

    [XmlIgnore]
    public bool SpeedOverGroundSpecified
    {
        get
        {
            return this.speedOverGroundFieldSpecified;
        }
        set
        {
            this.speedOverGroundFieldSpecified = value;
        }
    }

    public SignalQuality SignalQuality
    {
        get
        {
            return this.signalQualityField;
        }
        set
        {
            this.signalQualityField = value;
        }
    }

    [XmlIgnore]
    public bool SignalQualitySpecified
    {
        get
        {
            return this.signalQualityFieldSpecified;
        }
        set
        {
            this.signalQualityFieldSpecified = value;
        }
    }

    public Fix Fix
    {
        get
        {
            return this.fixField;
        }
        set
        {
            this.fixField = value;
        }
    }

    [XmlIgnore]
    public bool FixSpecified
    {
        get
        {
            return this.fixFieldSpecified;
        }
        set
        {
            this.fixFieldSpecified = value;
        }
    }

    [XmlElement(DataType = "int")]
    public int NumberOfSatellites
    {
        get
        {
            return this.numberOfSatellitesField;
        }
        set
        {
            this.numberOfSatellitesField = value;
        }
    }

    public double HorizontalDilutionOfPrecision
    {
        get
        {
            return this.horizontalDilutionOfPrecisionField;
        }
        set
        {
            this.horizontalDilutionOfPrecisionField = value;
        }
    }

    [XmlIgnore]
    public bool HorizontalDilutionOfPrecisionSpecified
    {
        get
        {
            return this.horizontalDilutionOfPrecisionFieldSpecified;
        }
        set
        {
            this.horizontalDilutionOfPrecisionFieldSpecified = value;
        }
    }

    public double VerticalDilutionOfPrecision
    {
        get
        {
            return this.verticalDilutionOfPrecisionField;
        }
        set
        {
            this.verticalDilutionOfPrecisionField = value;
        }
    }

    [XmlIgnore]
    public bool VerticalDilutionOfPrecisionSpecified
    {
        get
        {
            return this.verticalDilutionOfPrecisionFieldSpecified;
        }
        set
        {
            this.verticalDilutionOfPrecisionFieldSpecified = value;
        }
    }

    public double TrackDegreeTrue
    {
        get
        {
            return this.trackDegreeTrueField;
        }
        set
        {
            this.trackDegreeTrueField = value;
        }
    }

    [XmlIgnore]
    public bool TrackDegreeTrueSpecified
    {
        get
        {
            return this.trackDegreeTrueFieldSpecified;
        }
        set
        {
            this.trackDegreeTrueFieldSpecified = value;
        }
    }

    public double TrackDegreeMagnetic
    {
        get
        {
            return this.trackDegreeMagneticField;
        }
        set
        {
            this.trackDegreeMagneticField = value;
        }
    }

    [XmlIgnore]
    public bool TrackDegreeMagneticSpecified
    {
        get
        {
            return this.trackDegreeMagneticFieldSpecified;
        }
        set
        {
            this.trackDegreeMagneticFieldSpecified = value;
        }
    }

    public GNSSType GNSSType
    {
        get
        {
            return this.gNSSTypeField;
        }
        set
        {
            this.gNSSTypeField = value;
        }
    }

    public GNSSCoordinateSystem GNSSCoordinateSystem
    {
        get
        {
            return this.gNSSCoordinateSystemField;
        }
        set
        {
            this.gNSSCoordinateSystemField = value;
        }
    }

    [XmlIgnore]
    public bool GNSSCoordinateSystemSpecified
    {
        get
        {
            return this.gNSSCoordinateSystemFieldSpecified;
        }
        set
        {
            this.gNSSCoordinateSystemFieldSpecified = value;
        }
    }
}

[Serializable, XmlRoot(Namespace = "", IsNullable = false), DebuggerStepThrough, XmlType(AnonymousType = true), GeneratedCode("xsd", "2.0.50727.3038"), DesignerCategory("code")]
public class Latitude
{
    private double degreeField;
    private Direction directionField;

    public double Degree
    {
        get
        {
            return this.degreeField;
        }
        set
        {
            this.degreeField = value;
        }
    }

    public Direction Direction
    {
        get
        {
            return this.directionField;
        }
        set
        {
            this.directionField = value;
        }
    }
}

[Serializable, DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.3038"), XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true)]
public class Longitude
{
    private double degreeField;
    private Direction directionField;

    public double Degree
    {
        get
        {
            return this.degreeField;
        }
        set
        {
            this.degreeField = value;
        }
    }

    public Direction Direction
    {
        get
        {
            return this.directionField;
        }
        set
        {
            this.directionField = value;
        }
    }
}

[Serializable, GeneratedCode("xsd", "2.0.50727.3038"), XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true)]
public enum Direction
{
    N,
    S,
    E,
    W
}


[Serializable, XmlRoot(Namespace = "", IsNullable = false), GeneratedCode("xsd", "2.0.50727.3038"), XmlType(AnonymousType = true)]
public enum GNSSType
{
    GPS,
    Glonass,
    Galileo,
    Beidou,
    IRNSS,
    Other,
    DeadReckoning,
    MixedGNSSTypes
}

[Serializable, XmlRoot(Namespace = "", IsNullable = false), GeneratedCode("xsd", "2.0.50727.3038"), XmlType(AnonymousType = true)]
public enum GNSSCoordinateSystem
{
    CH1903,
    ETSR89,
    IERS,
    NAD27,
    NAD83,
    WGS84,
    WGS72,
    SGS85,
    P90
}

[Serializable, XmlType(AnonymousType = true), XmlRoot(Namespace = "", IsNullable = false), GeneratedCode("xsd", "2.0.50727.3038")]
public enum Fix
{
    NoFix = 0,
    [XmlEnum("2D")]
    Item2D = 1,
    [XmlEnum("3D")]
    Item3D = 2,
    DR = 3,
    [XmlEnum("3D+DR")]
    Item3DDR = 4
}

[Serializable, XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true), GeneratedCode("xsd", "2.0.50727.3038")]
public enum SignalQuality
{
    aGPS,
    dGPS,
    Estimated,
    GPS,
    NotValid,
    Unknown
}
