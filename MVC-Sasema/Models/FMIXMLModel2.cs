using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MVC_Sasema_test.Models3
{
			
		[XmlRoot(ElementName = "TimePeriod", Namespace = "http://www.opengis.net/gml/3.2")]
		public class TimePeriod
		{
			[XmlElement(ElementName = "beginPosition", Namespace = "http://www.opengis.net/gml/3.2")]
			public string BeginPosition { get; set; }
			[XmlElement(ElementName = "endPosition", Namespace = "http://www.opengis.net/gml/3.2")]
			public string EndPosition { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "phenomenonTime", Namespace = "http://www.opengis.net/om/2.0")]
		public class PhenomenonTime
		{
			[XmlElement(ElementName = "TimePeriod", Namespace = "http://www.opengis.net/gml/3.2")]
			public TimePeriod TimePeriod { get; set; }
			[XmlAttribute(AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
			public string Href { get; set; }
		}

		[XmlRoot(ElementName = "TimeInstant", Namespace = "http://www.opengis.net/gml/3.2")]
		public class TimeInstant
		{
			[XmlElement(ElementName = "timePosition", Namespace = "http://www.opengis.net/gml/3.2")]
			public string TimePosition { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "resultTime", Namespace = "http://www.opengis.net/om/2.0")]
		public class ResultTime
		{
			[XmlElement(ElementName = "TimeInstant", Namespace = "http://www.opengis.net/gml/3.2")]
			public TimeInstant TimeInstant { get; set; }
			[XmlAttribute(AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
			public string Href { get; set; }
		}

		[XmlRoot(ElementName = "procedure", Namespace = "http://www.opengis.net/om/2.0")]
		public class Procedure
		{
			[XmlAttribute(AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
			public string Href { get; set; }
		}

		[XmlRoot(ElementName = "name", Namespace = "http://www.opengis.net/om/2.0")]
		public class Name
		{
			[XmlAttribute(AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
			public string Href { get; set; }
		}

		[XmlRoot(ElementName = "value", Namespace = "http://www.opengis.net/om/2.0")]
		public class Value
		{
			[XmlElement(ElementName = "TimeInstant", Namespace = "http://www.opengis.net/gml/3.2")]
			public TimeInstant TimeInstant { get; set; }
		}

		[XmlRoot(ElementName = "NamedValue", Namespace = "http://www.opengis.net/om/2.0")]
		public class NamedValue
		{
			[XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/om/2.0")]
			public Name Name { get; set; }
			[XmlElement(ElementName = "value", Namespace = "http://www.opengis.net/om/2.0")]
			public Value Value { get; set; }
		}

		[XmlRoot(ElementName = "parameter", Namespace = "http://www.opengis.net/om/2.0")]
		public class Parameter
		{
			[XmlElement(ElementName = "NamedValue", Namespace = "http://www.opengis.net/om/2.0")]
			public NamedValue NamedValue { get; set; }
		}

		[XmlRoot(ElementName = "observedProperty", Namespace = "http://www.opengis.net/om/2.0")]
		public class ObservedProperty
		{
			[XmlAttribute(AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
			public string Href { get; set; }
		}

		[XmlRoot(ElementName = "identifier", Namespace = "http://www.opengis.net/gml/3.2")]
		public class Identifier
		{
			[XmlAttribute(AttributeName = "codeSpace")]
			public string CodeSpace { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "name", Namespace = "http://www.opengis.net/gml/3.2")]
		public class Name2
		{
			[XmlAttribute(AttributeName = "codeSpace")]
			public string CodeSpace { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "representativePoint", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
		public class RepresentativePoint
		{
			[XmlAttribute(AttributeName = "href", Namespace = "http://www.w3.org/1999/xlink")]
			public string Href { get; set; }
		}

		[XmlRoot(ElementName = "country", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
		public class Country
		{
			[XmlAttribute(AttributeName = "codeSpace")]
			public string CodeSpace { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "region", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
		public class Region
		{
			[XmlAttribute(AttributeName = "codeSpace")]
			public string CodeSpace { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Location", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
		public class Location
		{
			[XmlElement(ElementName = "identifier", Namespace = "http://www.opengis.net/gml/3.2")]
			public Identifier Identifier { get; set; }
			[XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/gml/3.2")]
			public List<Name2> Name2 { get; set; }
			[XmlElement(ElementName = "representativePoint", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public RepresentativePoint RepresentativePoint { get; set; }
			[XmlElement(ElementName = "country", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public Country Country { get; set; }
			[XmlElement(ElementName = "timezone", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public string Timezone { get; set; }
			[XmlElement(ElementName = "region", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public Region Region { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "member", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
		public class Member
		{
			[XmlElement(ElementName = "Location", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public Location Location { get; set; }
		}

		[XmlRoot(ElementName = "LocationCollection", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
		public class LocationCollection
		{
			[XmlElement(ElementName = "member", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public Member Member { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "sampledFeature", Namespace = "http://www.opengis.net/sampling/2.0")]
		public class SampledFeature
		{
			[XmlElement(ElementName = "LocationCollection", Namespace = "http://xml.fmi.fi/namespace/om/atmosphericfeatures/1.1")]
			public LocationCollection LocationCollection { get; set; }
		}

		[XmlRoot(ElementName = "Point", Namespace = "http://www.opengis.net/gml/3.2")]
		public class Point2
		{
			[XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Name2 { get; set; }
			[XmlElement(ElementName = "pos", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Pos { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
			[XmlAttribute(AttributeName = "srsName")]
			public string SrsName { get; set; }
			[XmlAttribute(AttributeName = "srsDimension")]
			public string SrsDimension { get; set; }
		}

		[XmlRoot(ElementName = "pointMembers", Namespace = "http://www.opengis.net/gml/3.2")]
		public class PointMembers
		{
			[XmlElement(ElementName = "Point", Namespace = "http://www.opengis.net/gml/3.2")]
			public Point Point { get; set; }
		}

		[XmlRoot(ElementName = "MultiPoint", Namespace = "http://www.opengis.net/gml/3.2")]
		public class MultiPoint
		{
			[XmlElement(ElementName = "pointMembers", Namespace = "http://www.opengis.net/gml/3.2")]
			public PointMembers PointMembers { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "shape", Namespace = "http://www.opengis.net/samplingSpatial/2.0")]
		public class Shape
		{
			[XmlElement(ElementName = "MultiPoint", Namespace = "http://www.opengis.net/gml/3.2")]
			public MultiPoint MultiPoint { get; set; }
		}

		[XmlRoot(ElementName = "SF_SpatialSamplingFeature", Namespace = "http://www.opengis.net/samplingSpatial/2.0")]
		public class SF_SpatialSamplingFeature
		{
			[XmlElement(ElementName = "sampledFeature", Namespace = "http://www.opengis.net/sampling/2.0")]
			public SampledFeature SampledFeature { get; set; }
			[XmlElement(ElementName = "shape", Namespace = "http://www.opengis.net/samplingSpatial/2.0")]
			public Shape Shape { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "featureOfInterest", Namespace = "http://www.opengis.net/om/2.0")]
		public class FeatureOfInterest
		{
			[XmlElement(ElementName = "SF_SpatialSamplingFeature", Namespace = "http://www.opengis.net/samplingSpatial/2.0")]
			public SF_SpatialSamplingFeature SF_SpatialSamplingFeature { get; set; }
		}

		[XmlRoot(ElementName = "MeasurementTVP", Namespace = "http://www.opengis.net/waterml/2.0")]
		public class MeasurementTVP
		{
			[XmlElement(ElementName = "time", Namespace = "http://www.opengis.net/waterml/2.0")]
			public DateTime Time { get; set; }
			[XmlElement(ElementName = "value", Namespace = "http://www.opengis.net/waterml/2.0")]
			public string Value2 { get; set; }
		}

		[XmlRoot(ElementName = "point", Namespace = "http://www.opengis.net/waterml/2.0")]
		public class Point
		{
			[XmlElement(ElementName = "MeasurementTVP", Namespace = "http://www.opengis.net/waterml/2.0")]
			public MeasurementTVP MeasurementTVP { get; set; }
		}

    [XmlRoot(ElementName = "MeasurementTimeseries", Namespace = "http://www.opengis.net/waterml/2.0")]
		public class MeasurementTimeseries
		{
			[XmlElement(ElementName = "point", Namespace = "http://www.opengis.net/waterml/2.0")]
			public List<Point> Point { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "result", Namespace = "http://www.opengis.net/om/2.0")]
		public class Result
		{
			[XmlElement(ElementName = "MeasurementTimeseries", Namespace = "http://www.opengis.net/waterml/2.0")]
			public MeasurementTimeseries MeasurementTimeseries { get; set; }
		}

		[XmlRoot(ElementName = "PointTimeSeriesObservation", Namespace = "http://inspire.ec.europa.eu/schemas/omso/3.0")]
		public class PointTimeSeriesObservation
		{
			[XmlElement(ElementName = "phenomenonTime", Namespace = "http://www.opengis.net/om/2.0")]
			public PhenomenonTime PhenomenonTime { get; set; }
			[XmlElement(ElementName = "resultTime", Namespace = "http://www.opengis.net/om/2.0")]
			public ResultTime ResultTime { get; set; }
			[XmlElement(ElementName = "procedure", Namespace = "http://www.opengis.net/om/2.0")]
			public Procedure Procedure { get; set; }
			[XmlElement(ElementName = "parameter", Namespace = "http://www.opengis.net/om/2.0")]
			public Parameter Parameter { get; set; }
			[XmlElement(ElementName = "observedProperty", Namespace = "http://www.opengis.net/om/2.0")]
			public ObservedProperty ObservedProperty { get; set; }
			[XmlElement(ElementName = "featureOfInterest", Namespace = "http://www.opengis.net/om/2.0")]
			public FeatureOfInterest FeatureOfInterest { get; set; }
			[XmlElement(ElementName = "result", Namespace = "http://www.opengis.net/om/2.0")]
			public Result Result { get; set; }
			[XmlAttribute(AttributeName = "id", Namespace = "http://www.opengis.net/gml/3.2")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "member", Namespace = "http://www.opengis.net/wfs/2.0")]
		public class Member2
		{
			[XmlElement(ElementName = "PointTimeSeriesObservation", Namespace = "http://inspire.ec.europa.eu/schemas/omso/3.0")]
			public PointTimeSeriesObservation PointTimeSeriesObservation { get; set; }
		}

		[XmlRoot(ElementName = "FeatureCollection", Namespace = "http://www.opengis.net/wfs/2.0")]
		public class FeatureCollection
		{
			[XmlElement(ElementName = "member", Namespace = "http://www.opengis.net/wfs/2.0")]
			public List<Member2> Member2 { get; set; }
			[XmlAttribute(AttributeName = "timeStamp")]
			public string TimeStamp { get; set; }
			[XmlAttribute(AttributeName = "numberMatched")]
			public string NumberMatched { get; set; }
			[XmlAttribute(AttributeName = "numberReturned")]
			public string NumberReturned { get; set; }
			[XmlAttribute(AttributeName = "wfs", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Wfs { get; set; }
			[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Xsi { get; set; }
			[XmlAttribute(AttributeName = "xlink", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Xlink { get; set; }
			[XmlAttribute(AttributeName = "om", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Om { get; set; }
			[XmlAttribute(AttributeName = "omso", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Omso { get; set; }
			[XmlAttribute(AttributeName = "ompr", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Ompr { get; set; }
			[XmlAttribute(AttributeName = "gml", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Gml { get; set; }
			[XmlAttribute(AttributeName = "gmd", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Gmd { get; set; }
			[XmlAttribute(AttributeName = "gco", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Gco { get; set; }
			[XmlAttribute(AttributeName = "swe", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Swe { get; set; }
			[XmlAttribute(AttributeName = "gmlcov", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Gmlcov { get; set; }
			[XmlAttribute(AttributeName = "sam", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Sam { get; set; }
			[XmlAttribute(AttributeName = "sams", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Sams { get; set; }
			[XmlAttribute(AttributeName = "wml2", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Wml2 { get; set; }
			[XmlAttribute(AttributeName = "target", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Target { get; set; }
			[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
			public string SchemaLocation { get; set; }
		}

}


