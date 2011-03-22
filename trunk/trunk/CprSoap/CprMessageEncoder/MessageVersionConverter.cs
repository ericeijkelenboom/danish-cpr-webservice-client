using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ServiceModel.Channels;

namespace CprSoap.CprMessageEncoder
{
    class MessageVersionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(string) == sourceType)
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (typeof(InstanceDescriptor) == destinationType)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                var messageVersion = (string)value;
                MessageVersion retval;
                switch (messageVersion)
                {
                    case Constants.Soap11WsAddressing10:
                        retval = MessageVersion.Soap11WSAddressing10;
                        break;
                    case Constants.Soap12WsAddressing10:
                        retval = MessageVersion.Soap12WSAddressing10;
                        break;
                    case Constants.Soap11WsAddressingAugust2004:
                        retval = MessageVersion.Soap11WSAddressingAugust2004;
                        break;
                    case Constants.Soap12WsAddressingAugust2004:
                        retval = MessageVersion.Soap12WSAddressingAugust2004;
                        break;
                    case Constants.Soap11:
                        retval = MessageVersion.Soap11;
                        break;
                    case Constants.Soap12:
                        retval = MessageVersion.Soap12;
                        break;
                    case Constants.None:
                        retval = MessageVersion.None;
                        break;
                    case Constants.Default:
                        retval = MessageVersion.Default;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("messageVersion");
                }

                return retval;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (typeof(string) == destinationType && value is MessageVersion)
            {
                string retval;
                var messageVersion = (MessageVersion)value;
                if (messageVersion == MessageVersion.Default)
                {
                    retval = Constants.Default;
                }
                else if (messageVersion == MessageVersion.Soap11WSAddressing10)
                {
                    retval = Constants.Soap11WsAddressing10;
                }
                else if (messageVersion == MessageVersion.Soap12WSAddressing10)
                {
                    retval = Constants.Soap12WsAddressing10;
                }
                else if (messageVersion == MessageVersion.Soap11WSAddressingAugust2004)
                {
                    retval = Constants.Soap11WsAddressingAugust2004;
                }
                else if (messageVersion == MessageVersion.Soap12WSAddressingAugust2004)
                {
                    retval = Constants.Soap12WsAddressingAugust2004;
                }
                else if (messageVersion == MessageVersion.Soap11)
                {
                    retval = Constants.Soap11;
                }
                else if (messageVersion == MessageVersion.Soap12)
                {
                    retval = Constants.Soap12;
                }
                else if (messageVersion == MessageVersion.None)
                {
                    retval = Constants.None;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("messageVersion");
                }
                return retval;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
