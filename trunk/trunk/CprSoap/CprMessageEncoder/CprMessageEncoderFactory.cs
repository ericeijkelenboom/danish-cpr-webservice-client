using System.ServiceModel.Channels;

namespace CprSoap.CprMessageEncoder
{
    public class CprMessageEncoderFactory : MessageEncoderFactory
    {
        private readonly string _charSet;
        private readonly MessageEncoder _encoder;
        private readonly string _mediaType;
        private readonly MessageVersion _version;

        internal CprMessageEncoderFactory(string mediaType, string charSet, MessageVersion version)
        {
            _version = version;
            _mediaType = mediaType;
            _charSet = charSet;
            _encoder = new CprMessageEncoder(this);
        }

        public override MessageEncoder Encoder
        {
            get { return _encoder; }
        }

        public override MessageVersion MessageVersion
        {
            get { return _version; }
        }

        internal string MediaType
        {
            get { return _mediaType; }
        }

        internal string CharSet
        {
            get { return _charSet; }
        }
    }
}