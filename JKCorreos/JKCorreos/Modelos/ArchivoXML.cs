using System.Xml.Serialization;

namespace JKCorreos.Modelos
{
    [XmlRoot(ElementName = "DespatchAdvice", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2")]
    public class DespatchAdvice
    {
        [XmlElement(ElementName = "UBLExtensions", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
        public UBLExtensions UBLExtensions { get; set; }

        [XmlElement(ElementName = "UBLVersionID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string UBLVersionID { get; set; }

        [XmlElement(ElementName = "CustomizationID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string CustomizationID { get; set; }

        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlElement(ElementName = "IssueDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string IssueDate { get; set; }

        [XmlElement(ElementName = "IssueTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string IssueTime { get; set; }

        [XmlElement(ElementName = "DespatchAdviceTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public DespatchAdviceTypeCode DespatchAdviceTypeCode { get; set; }

        [XmlElement(ElementName = "Signature", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Signature Signature { get; set; }

        [XmlElement(ElementName = "DespatchSupplierParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public DespatchSupplierParty DespatchSupplierParty { get; set; }

        [XmlElement(ElementName = "DeliveryCustomerParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public DeliveryCustomerParty DeliveryCustomerParty { get; set; }

        [XmlElement(ElementName = "Shipment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Shipment Shipment { get; set; }

        [XmlElement(ElementName = "DespatchLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public List<DespatchLine> DespatchLines { get; set; }
    }

    public class UBLExtensions
    {
        [XmlElement(ElementName = "UBLExtension", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
        public UBLExtension[] UBLExtension { get; set; }
    }

    public class UBLExtension
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
        [XmlElement(ElementName = "ExtensionContent", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
        public ExtensionContent ExtensionContent { get; set; }
    }

    public class ExtensionContent
    {
        [XmlElement("AdditionalInformation", Namespace = "urn:bizlinks:names:specification:ubl:peru:schema:xsd:BizlinksAggregateComponents-1")]
        public AdditionalInformation AdditionalInformation { get; set; }
    }

    public class AdditionalInformation
    {
        [XmlElement("AdditionalProperty", Namespace = "urn:bizlinks:names:specification:ubl:peru:schema:xsd:BizlinksAggregateComponents-1")]
        public AdditionalProperty[] AdditionalProperties { get; set; }
    }

    public class AdditionalProperty
    {
        [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlElement("Value", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string Value { get; set; }
    }

    public class DespatchAdviceTypeCode
    {
        [XmlAttribute(AttributeName = "listID")]
        public string ListID { get; set; }
        [XmlAttribute(AttributeName = "listAgencyName")]
        public string ListAgencyName { get; set; }
        [XmlAttribute(AttributeName = "listName")]
        public string ListName { get; set; }
        [XmlAttribute(AttributeName = "listVersionID")]
        public string ListVersionID { get; set; }
        [XmlText]
        public string Value { get; set; }
    }

    public class Signature
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
        [XmlElement(ElementName = "SignatoryParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public SignatoryParty SignatoryParty { get; set; }
        [XmlElement(ElementName = "DigitalSignatureAttachment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public DigitalSignatureAttachment DigitalSignatureAttachment { get; set; }
    }

    public class SignatoryParty
    {
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyIdentification PartyIdentification { get; set; }
    }

    public class DigitalSignatureAttachment
    {
        [XmlElement(ElementName = "ExternalReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public ExternalReference ExternalReference { get; set; }
    }

    public class ExternalReference
    {
        [XmlElement(ElementName = "URI", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string URI { get; set; }
    }

    public class DespatchSupplierParty
    {
        [XmlElement(ElementName = "Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Party Party { get; set; }
    }

    public class Party
    {
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyIdentification PartyIdentification { get; set; }
        [XmlElement(ElementName = "PartyName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyName PartyName { get; set; }
        [XmlElement(ElementName = "PartyTaxScheme", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyTaxScheme PartyTaxScheme { get; set; }
        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyLegalEntity PartyLegalEntity { get; set; }
    }

    public class PartyName
    {
        [XmlElement(ElementName = "Name", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string Name { get; set; }
    }

    public class PartyTaxScheme
    {
        [XmlElement(ElementName = "TaxScheme", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public TaxScheme TaxScheme { get; set; }
    }

    public class TaxScheme
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
    }

    public class DeliveryCustomerParty
    {
        [XmlElement(ElementName = "Party", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Party Party { get; set; }
    }

    public class Shipment
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlElement(ElementName = "HandlingCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public HandlingCode HandlingCode { get; set; }

        [XmlElement(ElementName = "HandlingInstructions", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string HandlingInstructions { get; set; }

        [XmlElement(ElementName = "GrossWeightMeasure", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public GrossWeightMeasure GrossWeightMeasure { get; set; }

        [XmlElement(ElementName = "TotalTransportHandlingUnitQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string TotalTransportHandlingUnitQuantity { get; set; }

        [XmlElement(ElementName = "ShipmentStage", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public ShipmentStage ShipmentStage { get; set; }

        [XmlElement(ElementName = "Delivery", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Delivery Delivery { get; set; }

        [XmlElement("TransportHandlingUnit", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public TransportHandlingUnitModel TransportHandlingUnit { get; set; }

    }

    public class HandlingCode
    {
        [XmlAttribute(AttributeName = "listAgencyName")]
        public string ListAgencyName { get; set; }

        [XmlAttribute(AttributeName = "listName")]
        public string ListName { get; set; }

        [XmlAttribute(AttributeName = "listURI")]
        public string ListURI { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class GrossWeightMeasure
    {
        [XmlAttribute(AttributeName = "unitCode")]
        public string UnitCode { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class ShipmentStage
    {
        [XmlElement(ElementName = "TransportModeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public TransportModeCode TransportModeCode { get; set; }

        [XmlElement(ElementName = "TransitPeriod", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public TransitPeriod TransitPeriod { get; set; }

        [XmlElement(ElementName = "CarrierParty", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public CarrierParty CarrierParty { get; set; } }
    }

    public class TransportModeCode
    {
        [XmlAttribute(AttributeName = "listAgencyName")]
        public string ListAgencyName { get; set; }

        [XmlAttribute(AttributeName = "listName")]
        public string ListName { get; set; }

        [XmlAttribute(AttributeName = "listURI")]
        public string ListURI { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class TransitPeriod
    {
        [XmlElement(ElementName = "StartDate", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string StartDate { get; set; }
    }

    public class CarrierParty
    {
        [XmlElement(ElementName = "PartyIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyIdentification PartyIdentification { get; set; }

        [XmlElement(ElementName = "PartyLegalEntity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public PartyLegalEntity PartyLegalEntity { get; set; }
    }

    public class DriverPersonModel
    {
        [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public IDModel ID { get; set; }

        [XmlElement("FirstName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string FirstName { get; set; }

        [XmlElement("FamilyName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string FamilyName { get; set; }

        [XmlElement("JobTitle", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string JobTitle { get; set; }

        [XmlElement("IdentityDocumentReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public IdentityDocumentReferenceModel IdentityDocumentReference { get; set; }
    }

    public class IDModel
    {
        [XmlAttribute("schemeAgencyName")]
        public string SchemeAgencyName { get; set; }

        [XmlAttribute("schemeID")]
        public string SchemeID { get; set; }

        [XmlAttribute("schemeName")]
        public string SchemeName { get; set; }

        [XmlAttribute("schemeURI")]
        public string SchemeURI { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class IdentityDocumentReferenceModel
    {
        [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
    }

    public class TransportHandlingUnitModel
    {
        [XmlElement("TransportEquipment", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public TransportEquipmentModel TransportEquipment { get; set; }
    }

    public class TransportEquipmentModel
    {
        [XmlElement("ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
    }

    public class PartyIdentification
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "schemeAgencyName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SchemeAgencyName { get; set; }

        [XmlAttribute(AttributeName = "schemeID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SchemeID { get; set; }

        [XmlAttribute(AttributeName = "schemeName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SchemeName { get; set; }

        [XmlAttribute(AttributeName = "schemeURI", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string SchemeURI { get; set; }
    }
    
    public class PartyLegalEntity
    {
        [XmlElement(ElementName = "RegistrationName", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string RegistrationName { get; set; }
    }

    public class Delivery
    {
        [XmlElement(ElementName = "DeliveryAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public DeliveryAddress DeliveryAddress { get; set; }

        [XmlElement(ElementName = "Despatch", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Despatch Despatch { get; set; }
    }

    public class DeliveryAddress
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "schemeAgencyName")]
        public string SchemeAgencyName { get; set; }

        [XmlAttribute(AttributeName = "schemeName")]
        public string SchemeName { get; set; }

        [XmlElement(ElementName = "AddressTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public AddressTypeCode AddressTypeCode { get; set; }

        [XmlElement(ElementName = "AddressLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public AddressLine AddressLine { get; set; }
    }

    public class AddressTypeCode
    {
        [XmlAttribute(AttributeName = "listAgencyName")]
        public string ListAgencyName { get; set; }

        [XmlAttribute(AttributeName = "listID")]
        public string ListID { get; set; }

        [XmlAttribute(AttributeName = "listName")]
        public string ListName { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class AddressLine
    {
        [XmlElement(ElementName = "Line", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string Line { get; set; }
    }

    public class Despatch
    {
        [XmlElement(ElementName = "DespatchAddress", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public DespatchAddress DespatchAddress { get; set; }
    }

    public class DespatchAddress
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "schemeAgencyName")]
        public string SchemeAgencyName { get; set; }

        [XmlAttribute(AttributeName = "schemeName")]
        public string SchemeName { get; set; }

        [XmlElement(ElementName = "AddressTypeCode", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public AddressTypeCode AddressTypeCode { get; set; }

        [XmlElement(ElementName = "AddressLine", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public AddressLine AddressLine { get; set; }
    }

    public class TransportMeans
    {
        [XmlElement(ElementName = "RoadTransport", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public RoadTransport RoadTransport { get; set; }
    }

    public class RoadTransport
    {
        [XmlElement(ElementName = "LicensePlateID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string LicensePlateID { get; set; }
    }

    public class DespatchLine
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }

        [XmlElement(ElementName = "DeliveredQuantity", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public DeliveredQuantity DeliveredQuantity { get; set; }

        [XmlElement(ElementName = "OrderLineReference", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public OrderLineReference OrderLineReference { get; set; }

        [XmlElement(ElementName = "Item", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public Item Item { get; set; }
    }

    public class DeliveredQuantity
    {
        [XmlAttribute(AttributeName = "unitCode")]
        public string UnitCode { get; set; }

        [XmlAttribute(AttributeName = "unitCodeListAgencyName")]
        public string UnitCodeListAgencyName { get; set; }

        [XmlAttribute(AttributeName = "unitCodeListID")]
        public string UnitCodeListID { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class OrderLineReference
    {
        [XmlElement(ElementName = "LineID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string LineID { get; set; }
    }

    public class Item
    {
        [XmlElement(ElementName = "Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string Description { get; set; }

        [XmlElement(ElementName = "SellersItemIdentification", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
        public SellersItemIdentification SellersItemIdentification { get; set; }
    }

    public class SellersItemIdentification
    {
        [XmlElement(ElementName = "ID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public string ID { get; set; }
    }

