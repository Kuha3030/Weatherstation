using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Sasema_test.Models
{
    // NOT USED ATM! Can be removed.

    public class FMIObject4
    {
        public Xml xml { get; set; }
        public WfsFeaturecollection wfsFeatureCollection { get; set; }
    }

    public class Xml
    {
        public string version { get; set; }
        public string encoding { get; set; }
    }

    public class CopyOfXml
    {
        public string version { get; set; }
        public string encoding { get; set; }
    }

    public class CopyOfWfsFeaturecollection
    {
        public DateTime timeStamp { get; set; }
        public string numberMatched { get; set; }
        public string numberReturned { get; set; }
        public string xmlnswfs { get; set; }
        public string xmlnsxsi { get; set; }
        public string xmlnsxlink { get; set; }
        public string xmlnsom { get; set; }
        public string xmlnsomso { get; set; }
        public string xmlnsompr { get; set; }
        public string xmlnsgml { get; set; }
        public string xmlnsgmd { get; set; }
        public string xmlnsgco { get; set; }
        public string xmlnsswe { get; set; }
        public string xmlnsgmlcov { get; set; }
        public string xmlnssam { get; set; }
        public string xmlnssams { get; set; }
        public string xmlnswml2 { get; set; }
        public string xmlnstarget { get; set; }
        public string xsischemaLocation { get; set; }
        public WfsMember wfsmember { get; set; }
    }

    public class WfsFeaturecollection
    {
        public DateTime timeStamp { get; set; }
        public string numberMatched { get; set; }
        public string numberReturned { get; set; }
        public string xmlnswfs { get; set; }
        public string xmlnsxsi { get; set; }
        public string xmlnsxlink { get; set; }
        public string xmlnsom { get; set; }
        public string xmlnsomso { get; set; }
        public string xmlnsompr { get; set; }
        public string xmlnsgml { get; set; }
        public string xmlnsgmd { get; set; }
        public string xmlnsgco { get; set; }
        public string xmlnsswe { get; set; }
        public string xmlnsgmlcov { get; set; }
        public string xmlnssam { get; set; }
        public string xmlnssams { get; set; }
        public string xmlnswml2 { get; set; }
        public string xmlnstarget { get; set; }
        public string xsischemaLocation { get; set; }
        public WfsMember wfsmember { get; set; }
    }

    public class CopyOfWfsMember
    {
        public OmsoPointtimeseriesobservation omsoPointTimeSeriesObservation { get; set; }
    }

    public class WfsMember
    {
        public OmsoPointtimeseriesobservation omsoPointTimeSeriesObservation { get; set; }
    }

    public class OmsoPointtimeseriesobservation
    {
        public string gmlid { get; set; }
        public OmPhenomenontime omphenomenonTime { get; set; }
        public OmResulttime omresultTime { get; set; }
        public OmProcedure omprocedure { get; set; }
        public OmParameter omparameter { get; set; }
        public OmObservedproperty omobservedProperty { get; set; }
        public OmFeatureofinterest omfeatureOfInterest { get; set; }
        public OmResult omresult { get; set; }
    }

    public class OmPhenomenontime
    {
        public GmlTimeperiod gmlTimePeriod { get; set; }
    }

    public class GmlTimeperiod
    {
        public string gmlid { get; set; }
        public DateTime gmlbeginPosition { get; set; }
        public DateTime gmlendPosition { get; set; }
    }

    public class OmResulttime
    {
        public GmlTimeinstant gmlTimeInstant { get; set; }
    }

    public class GmlTimeinstant
    {
        public string gmlid { get; set; }
        public DateTime gmltimePosition { get; set; }
    }

    public class OmProcedure
    {
        public string xlinkhref { get; set; }
    }

    public class OmParameter
    {
        public OmNamedvalue omNamedValue { get; set; }
    }

    public class OmNamedvalue
    {
        public OmName omname { get; set; }
        public OmValue omvalue { get; set; }
    }

    public class OmName
    {
        public string xlinkhref { get; set; }
    }

    public class OmValue
    {
        public GmlTimeinstant1 gmlTimeInstant { get; set; }
    }

    public class GmlTimeinstant1
    {
        public string gmlid { get; set; }
        public DateTime gmltimePosition { get; set; }
    }

    public class OmObservedproperty
    {
        public string xlinkhref { get; set; }
    }

    public class OmFeatureofinterest
    {
        public SamsSF_Spatialsamplingfeature samsSF_SpatialSamplingFeature { get; set; }
    }

    public class SamsSF_Spatialsamplingfeature
    {
        public string gmlid { get; set; }
        public SamSampledfeature samsampledFeature { get; set; }
        public SamsShape samsshape { get; set; }
    }

    public class SamSampledfeature
    {
        public TargetLocationcollection targetLocationCollection { get; set; }
    }

    public class TargetLocationcollection
    {
        public string gmlid { get; set; }
        public TargetMember targetmember { get; set; }
    }

    public class TargetMember
    {
        public TargetLocation targetLocation { get; set; }
    }

    public class TargetLocation
    {
        public string gmlid { get; set; }
        public GmlIdentifier gmlidentifier { get; set; }
        public GmlName[] gmlname { get; set; }
        public TargetRepresentativepoint targetrepresentativePoint { get; set; }
        public TargetCountry targetcountry { get; set; }
        public string targettimezone { get; set; }
        public TargetRegion targetregion { get; set; }
    }

    public class GmlIdentifier
    {
        public string codeSpace { get; set; }
        public string text { get; set; }
    }

    public class TargetRepresentativepoint
    {
        public string xlinkhref { get; set; }
    }

    public class TargetCountry
    {
        public string codeSpace { get; set; }
        public string text { get; set; }
    }

    public class TargetRegion
    {
        public string codeSpace { get; set; }
        public string text { get; set; }
    }

    public class GmlName
    {
        public string codeSpace { get; set; }
        public string text { get; set; }
    }

    public class SamsShape
    {
        public GmlMultipoint gmlMultiPoint { get; set; }
    }

    public class GmlMultipoint
    {
        public string gmlid { get; set; }
        public GmlPointmembers gmlpointMembers { get; set; }
    }

    public class GmlPointmembers
    {
        public GmlPoint gmlPoint { get; set; }
    }

    public class GmlPoint
    {
        public string gmlid { get; set; }
        public string srsName { get; set; }
        public string srsDimension { get; set; }
        public string gmlname { get; set; }
        public string gmlpos { get; set; }
    }

    public class OmResult
    {
        public Wml2Measurementtimeseries wml2MeasurementTimeseries { get; set; }
    }

    public class CopyOfWml2Measurementtimeseries
    {
        public string gmlid { get; set; }
        public Wml2Point[] wml2point { get; set; }
    }

    public class Wml2Measurementtimeseries
    {
        public string gmlid { get; set; }
        public Wml2Point[] wml2point { get; set; }
    }

    public class CopyOfWml2Point
    {
        public Wml2Measurementtvp wml2MeasurementTVP { get; set; }
    }

    public class CopyOfWml2Measurementtvp
    {
        public DateTime wml2time { get; set; }
        public string wml2value { get; set; }
    }

    public class Wml2Point
    {
        public Wml2Measurementtvp wml2MeasurementTVP { get; set; }
    }

    public class Wml2Measurementtvp
    {
        public DateTime wml2time { get; set; }
        public string wml2value { get; set; }
    }




}
