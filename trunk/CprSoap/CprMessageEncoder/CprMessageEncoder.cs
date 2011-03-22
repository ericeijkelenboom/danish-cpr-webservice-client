using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CprSoap.CprMessageEncoder
{
    public class CprMessageEncoder : MessageEncoder
    {
        private readonly string _contentType;
        private readonly CprMessageEncoderFactory _factory;
        private readonly XmlWriterSettings _writerSettings;

        public CprMessageEncoder(CprMessageEncoderFactory factory)
        {
            _factory = factory;

            _writerSettings = new XmlWriterSettings {Encoding = Encoding.GetEncoding(factory.CharSet)};
            _contentType = string.Format("{0};charset={1}", _factory.MediaType, _writerSettings.Encoding.HeaderName);
        }

        public override string ContentType
        {
            get { return _contentType; }
        }

        public override string MediaType
        {
            get { return _factory.MediaType; }
        }

        public override MessageVersion MessageVersion
        {
            get { return _factory.MessageVersion; }
        }

        /* 
         * This is where the magic happens. The entire WS-Security SOAP header is removed from the response. 
         * Reasons: 
         * 1. It contains a WSS 1.1 attribute and the client is configured with WSS 1.1
         * 2. It contains an unsigned timestamp. WCF does not like this, since the client is configured to use WSS. 
         */
        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            var msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            string message = Encoding.UTF8.GetString(msgContents);
            string modifiedMessage = Regex.Replace(message, @"<wsse:Security.*</wsse:Security>", "", RegexOptions.Multiline);

            byte[] newMessageContents = Encoding.UTF8.GetBytes(modifiedMessage);

            var stream = new MemoryStream(newMessageContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlReader reader = XmlReader.Create(stream);
            return Message.CreateMessage(reader, maxSizeOfHeaders, MessageVersion);
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager,
                                                        int messageOffset)
        {
            var stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, _writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            var messageLength = (int) stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            var byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, _writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }
}