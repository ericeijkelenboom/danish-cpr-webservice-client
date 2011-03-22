using System;
using System.ComponentModel;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Xml;

namespace CprSoap.CprMessageEncoder
{
    public class CprMessageEncodingElement : BindingElementExtensionElement
    {
        public override Type BindingElementType
        {
            get { return typeof (CprMessageBindingElement); }
        }

        [ConfigurationProperty(Constants.MessageVersion, DefaultValue = Constants.DefaultMessageVersion)]
        [TypeConverter(typeof (MessageVersionConverter))]
        public MessageVersion MessageVersion
        {
            get { return (MessageVersion) base[Constants.MessageVersion]; }
            set { base[Constants.MessageVersion] = value; }
        }

        [ConfigurationProperty(Constants.MediaType, DefaultValue = Constants.DefaultMediaType)]
        public string MediaType
        {
            get { return (string)base[Constants.MediaType]; }

            set { base[Constants.MediaType] = value; }
        }

        [ConfigurationProperty(Constants.Encoding, DefaultValue = Constants.DefaultEncoding)]
        public string Encoding
        {
            get { return (string) base[Constants.Encoding]; }

            set { base[Constants.Encoding] = value; }
        }

        [ConfigurationProperty(Constants.ReaderQuotas)]
        public XmlDictionaryReaderQuotasElement ReaderQuotasElement
        {
            get { return (XmlDictionaryReaderQuotasElement) base[Constants.ReaderQuotas]; }
        }

        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            var binding = (CprMessageBindingElement)bindingElement;
            binding.MessageVersion = MessageVersion;
            binding.MediaType = MediaType;
            binding.Encoding = Encoding;
            ApplyConfiguration(binding.ReaderQuotas);
        }

        private void ApplyConfiguration(XmlDictionaryReaderQuotas readerQuotas)
        {
            if (readerQuotas == null)
                throw new ArgumentNullException("readerQuotas");

            if (ReaderQuotasElement.MaxDepth != 0)
            {
                readerQuotas.MaxDepth = ReaderQuotasElement.MaxDepth;
            }
            if (ReaderQuotasElement.MaxStringContentLength != 0)
            {
                readerQuotas.MaxStringContentLength = ReaderQuotasElement.MaxStringContentLength;
            }
            if (ReaderQuotasElement.MaxArrayLength != 0)
            {
                readerQuotas.MaxArrayLength = ReaderQuotasElement.MaxArrayLength;
            }
            if (ReaderQuotasElement.MaxBytesPerRead != 0)
            {
                readerQuotas.MaxBytesPerRead = ReaderQuotasElement.MaxBytesPerRead;
            }
            if (ReaderQuotasElement.MaxNameTableCharCount != 0)
            {
                readerQuotas.MaxNameTableCharCount = ReaderQuotasElement.MaxNameTableCharCount;
            }
        }

        protected override BindingElement CreateBindingElement()
        {
            var binding = new CprMessageBindingElement();
            ApplyConfiguration(binding);
            return binding;
        }
    }
}