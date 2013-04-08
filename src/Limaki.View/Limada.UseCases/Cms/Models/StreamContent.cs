using System.IO;
using Limaki.Model.Content;

namespace Limada.Usecases.Cms.Models {
    public class StreamContent : Content<Stream> {
        public StreamContent () { }
        public StreamContent (Content content):base(content){}
        public string MimeType { get; set; }
    }
}